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
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirma tu correo - PyrosFit</title>
  </head>
  <body style=""margin: 0; padding: 0; background-color: #080808; font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; -webkit-font-smoothing: antialiased;"">
    
    <!-- Contenedor Principal -->
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""background-color: #080808; padding: 40px 10px;"">
      <tr>
        <td align=""center"">
          
          <!-- Tarjeta Principal -->
          <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""max-width: 540px; background-color: #121212; border: 1px solid #222222; border-top: 4px solid #ff5500; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 30px rgba(255, 85, 0, 0.08);"">
            
            <!-- Header con Branding -->
            <tr>
              <td style=""padding: 36px 36px 20px 36px;"">
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                  <tr>
                    <td>
                      <span style=""font-size: 24px; font-weight: 900; color: #ffffff; letter-spacing: -0.5px; text-transform: uppercase;"">
                        PYROS<span style=""color: #ff5500;"">FIT</span>
                      </span>
                      <div style=""font-size: 11px; color: #ff5500; font-weight: 700; text-transform: uppercase; letter-spacing: 1.5px; margin-top: 2px;"">
                        Activa tu cuenta
                      </div>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>

            <!-- Línea Separadora Sutil -->
            <tr>
              <td style=""padding: 0 36px;"">
                <div style=""border-bottom: 1px solid #1f1f1f; width: 100%;""></div>
              </td>
            </tr>

            <!-- Contenido Principal -->
            <tr>
              <td style=""padding: 28px 36px 36px 36px;"">
                <h1 style=""font-size: 22px; font-weight: 700; color: #ffffff; margin: 0 0 12px 0; letter-spacing: -0.3px;"">
                  ¡Estás a un paso de empezar! 🔥
                </h1>
                
                <p style=""color: #a0a0a0; font-size: 15px; line-height: 1.6; margin: 0 0 28px 0;"">
                  Gracias por unirte a <strong style=""color: #ffffff;"">PyrosFit</strong>. Para activar tu perfil y acceder a la plataforma, confirma tu dirección de correo presionando el botón de abajo.
                </p>

                <!-- Botón CTA de Alto Impacto -->
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                  <tr>
                    <td align=""center"" style=""padding: 8px 0 24px 0;"">
                      <a href=""{link}"" target=""_blank"" style=""display: inline-block; background: linear-gradient(135deg, #ff6b00 0%, #e64a00 100%); color: #ffffff; font-size: 15px; font-weight: 800; text-decoration: none; padding: 16px 36px; border-radius: 10px; text-transform: uppercase; letter-spacing: 1px; box-shadow: 0 6px 20px rgba(255, 107, 0, 0.35);"">
                        ACTIVAR MI CUENTA &rarr;
                      </a>
                    </td>
                  </tr>
                </table>

                <!-- Aviso de Expiración -->
                <div style=""background-color: #171717; border-left: 3px solid #ff5500; border-radius: 4px; padding: 12px 16px; margin-top: 12px;"">
                  <p style=""color: #888888; font-size: 12px; line-height: 1.5; margin: 0;"">
                    ⏱️ Este enlace es personal y expirará en <strong style=""color: #cccccc;"">24 horas</strong>. Si no creaste esta cuenta, puedes ignorar este mensaje.
                  </p>
                </div>

              </td>
            </tr>

            <!-- Footer Interno -->
            <tr>
              <td style=""background-color: #0d0d0d; padding: 20px 36px; text-align: center; border-top: 1px solid #1a1a1a;"">
                <p style=""color: #555555; font-size: 11px; margin: 0;"">
                  &copy; PyrosFit. Todos los derechos reservados.
                </p>
              </td>
            </tr>

          </table>

        </td>
      </tr>
    </table>

  </body>
</html>";
        }
    }
}
