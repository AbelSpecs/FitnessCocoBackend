using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO.Payments;
using InsightCore.Application.Interface.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InsightCore.Infrastructure.Payments.PayPal
{
    public class PayPalService : IPayPalService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PayPalService> _logger;

        private string? _accessToken;
        private DateTime _accessTokenExpiresAt = DateTime.MinValue;

        public PayPalService(IHttpClientFactory httpFactory, IConfiguration configuration, ILogger<PayPalService> logger)
        {
            _httpFactory = httpFactory;
            _configuration = configuration;
            _logger = logger;
        }

        private string BaseUrl => (_configuration["PayPal:UseSandbox"] ?? "true").ToLower() == "true" ? "https://api-m.sandbox.paypal.com" : "https://api-m.paypal.com";

        private async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_accessToken) && _accessTokenExpiresAt > DateTime.UtcNow.AddSeconds(30))
                return _accessToken;

            var clientId = _configuration["PayPal:ClientId"];
            var secret = _configuration["PayPal:ClientSecret"];
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(secret))
            {
                _logger.LogError("PayPal credentials not configured");
                return null;
            }

            var client = _httpFactory.CreateClient();
            var tokenUrl = new Uri(new Uri(BaseUrl), "/v1/oauth2/token");

            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secret}"));
            using var req = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
            req.Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });

            using var resp = await client.SendAsync(req, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Failed to obtain PayPal token: {Status} {Body}", resp.StatusCode, body);
                return null;
            }

            var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            if (doc.RootElement.TryGetProperty("access_token", out var at))
            {
                _accessToken = at.GetString();
                if (doc.RootElement.TryGetProperty("expires_in", out var ex))
                {
                    var seconds = ex.GetInt32();
                    _accessTokenExpiresAt = DateTime.UtcNow.AddSeconds(seconds);
                }
                return _accessToken;
            }

            return null;
        }

        public async Task<PayPalCreateOrderResult?> CreateOrderAsync(PayPalCreateOrderRequest request, CancellationToken cancellationToken)
        {
            var token = await GetAccessTokenAsync(cancellationToken);
            if (string.IsNullOrEmpty(token)) return null;

            var client = _httpFactory.CreateClient();
            var url = new Uri(new Uri(BaseUrl), "/v2/checkout/orders");
            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[] {
                    new {
                        amount = new {
                            currency_code = request.Currency,
                            value = request.Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                        },
                        description = request.Description
                    }
                },
                application_context = new {
                    return_url = request.ReturnUrl,
                    cancel_url = request.CancelUrl
                }
            };

            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using var resp = await client.SendAsync(req, cancellationToken);
            var content = await resp.Content.ReadAsStringAsync(cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("PayPal CreateOrder failed: {Status} {Content}", resp.StatusCode, content);
                return null;
            }

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            var orderId = root.GetProperty("id").GetString();
            string? approveUrl = null;
            if (root.TryGetProperty("links", out var links))
            {
                foreach (var l in links.EnumerateArray())
                {
                    if (l.GetProperty("rel").GetString() == "approve")
                    {
                        approveUrl = l.GetProperty("href").GetString();
                        break;
                    }
                }
            }

            if (orderId == null || approveUrl == null) return null;
            return new PayPalCreateOrderResult(orderId, approveUrl);
        }

        public async Task<PayPalCaptureOrderResult?> CaptureOrderAsync(string orderId, CancellationToken cancellationToken)
        {
            var token = await GetAccessTokenAsync(cancellationToken);
            if (string.IsNullOrEmpty(token)) return null;

            var client = _httpFactory.CreateClient();
            var url = new Uri(new Uri(BaseUrl), $"/v2/checkout/orders/{Uri.EscapeDataString(orderId)}/capture");
            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var resp = await client.SendAsync(req, cancellationToken);
            var content = await resp.Content.ReadAsStringAsync(cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("PayPal Capture failed for {OrderId}: {Status} {Content}", orderId, resp.StatusCode, content);
                return null;
            }

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            var status = root.GetProperty("status").GetString() ?? "";
            string? captureId = null;
            if (root.TryGetProperty("purchase_units", out var pus))
            {
                foreach (var pu in pus.EnumerateArray())
                {
                    if (pu.TryGetProperty("payments", out var payments) && payments.TryGetProperty("captures", out var caps))
                    {
                        foreach (var c in caps.EnumerateArray())
                        {
                            if (c.TryGetProperty("id", out var id))
                            {
                                captureId = id.GetString();
                                break;
                            }
                        }
                    }
                    if (captureId != null) break;
                }
            }

            return new PayPalCaptureOrderResult(orderId, status ?? string.Empty, captureId);
        }
    }
}
