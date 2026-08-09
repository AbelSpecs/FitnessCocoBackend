using InsightCore.Application.Interface.Presentation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace InsightCore.Infrastructure.Notification
{
    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _fromEmail;
        private readonly ILogger<EmailService> _logger;
        private readonly string _resendApiKey;

        public EmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<EmailService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _fromEmail = configuration["AppSettings:FromEmail"] ?? "no-reply@pyrosfit.com";
            _resendApiKey = configuration["Resend:ApiKey"] ?? string.Empty;
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string confirmationLink)
        {
            var html = BuildHtml(confirmationLink);

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails")
            {
                Content = JsonContent.Create(new
                {
                    from = _fromEmail,
                    to = new[] { toEmail },
                    subject = "Confirma tu correo - PyrosFit",
                    html = html
                }, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            if (!string.IsNullOrWhiteSpace(_resendApiKey))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _resendApiKey);
            }

            try
            {
                var resp = await client.SendAsync(request);
                if (resp.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email de confirmación enviado a {Email}", toEmail);
                }
                else
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    _logger.LogError("Fallo al enviar email a {Email}. Status: {Status}. Body: {Body}", toEmail, resp.StatusCode, body);
                    resp.EnsureSuccessStatusCode();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error enviando email de confirmación a {Email}", toEmail);
                throw;
            }
        }

        private string BuildHtml(string link)
        {
            // Plantilla HTML responsive, modo oscuro con acento naranja #ff6b00
            return $@"<!doctype html>
<html lang=""es""> 
  <head>
    <meta charset=""utf-8""> 
    <meta name=""viewport"" content=""width=device-width,initial-scale=1""> 
    <title>Confirma tu correo - PyrosFit</title>
    <style>
      body {{ margin:0; padding:0; background:#0b0b0b; color:#e5e5e5; font-family:Inter, system-ui, -apple-system, 'Segoe UI', Roboto, 'Helvetica Neue', Arial; }}
      .container {{ max-width:640px; margin:0 auto; padding:24px; }}
      .card {{ background:linear-gradient(180deg,#0f0f0f,#141414); border-radius:12px; padding:32px; box-shadow:0 6px 18px rgba(0,0,0,0.6); }}
      .brand {{ display:flex; align-items:center; gap:12px; margin-bottom:18px; }}
      .brand-logo {{ width:48px; height:48px; background:#111; border-radius:8px; display:inline-block; }}
      .title {{ font-size:20px; font-weight:600; color:#fff; margin-bottom:8px; }}
      .desc {{ color:#bfbfbf; line-height:1.5; margin-bottom:20px; }}
      .btn {{ display:inline-block; background:#ff6b00; color:#fff; padding:14px 22px; border-radius:10px; text-decoration:none; font-weight:600; }}
      .small {{ color:#9a9a9a; font-size:13px; margin-top:18px; }}
      @media (max-width:480px) {{ .container{{padding:16px}} .card{{padding:20px}} }}
    </style>
  </head>
  <body>
    <div class=""container"">
      <div class=""card"">
        <div class=""brand"">
          <div class=""brand-logo""></div>
          <div>
            <div style=""color:#ff6b00;font-weight:700;letter-spacing:0.5px"">PyrosFit</div>
            <div style=""font-size:12px;color:#9a9a9a"">Activa tu cuenta</div>
          </div>
        </div>
        <h1 class=""title"">Confirma tu correo</h1>
        <p class=""desc"">Gracias por registrarte en PyrosFit. Pulsa el botón de abajo para activar tu cuenta y empezar a usar la aplicación.</p>
        <p style=""text-align:center;margin:28px 0"">
          <a class=""btn"" href=""{link}"" target=""_blank"" rel=""noopener noreferrer"">Activar mi cuenta</a>
        </p>
        <p class=""small"">Si no reconoces esta cuenta, puedes ignorar este correo. El enlace expirará en 24 horas.</p>
      </div>
    </div>
  </body>
</html>";
        }
    }
}
