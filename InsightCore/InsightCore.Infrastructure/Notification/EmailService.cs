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
            var html = BuildConfirmationHtml(confirmationLink);
            await SendEmailAsync(toEmail, "Confirma tu correo - PyrosFit", html);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink, string? userName = null)
        {
            var html = BuildPasswordResetHtml(resetLink, userName);
            await SendEmailAsync(toEmail, "Restablece tu contraseña - PyrosFit", html);
        }

        public async Task SendMotivationEmailAsync(string toEmail, string studentName, string message, string? coachName = null)
        {
            var html = BuildMotivationHtml(studentName, message, coachName);
            await SendEmailAsync(toEmail, "¡Tu entrenador te ha enviado un mensaje! 🔥 - PyrosFit", html);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string html)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails")
            {
                Content = JsonContent.Create(new
                {
                    from = _fromEmail,
                    to = new[] { toEmail },
                    subject = subject,
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
                    _logger.LogInformation("Email '{Subject}' enviado con éxito a {Email}", subject, toEmail);
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
                _logger.LogError(ex, "Error enviando email '{Subject}' a {Email}", subject, toEmail);
                throw;
            }
        }

        private string BuildConfirmationHtml(string link)
        {
            return $@"<!doctype html>
<html lang=""es"">
  <head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirma tu correo - PyrosFit</title>
  </head>
  <body style=""margin: 0; padding: 0; background-color: #080808; font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; -webkit-font-smoothing: antialiased;"">
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""background-color: #080808; padding: 40px 10px;"">
      <tr>
        <td align=""center"">
          <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""max-width: 540px; background-color: #121212; border: 1px solid #222222; border-top: 4px solid #ff5500; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 30px rgba(255, 85, 0, 0.08);"">
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
            <tr>
              <td style=""padding: 0 36px;"">
                <div style=""border-bottom: 1px solid #1f1f1f; width: 100%;""></div>
              </td>
            </tr>
            <tr>
              <td style=""padding: 28px 36px 36px 36px;"">
                <h1 style=""font-size: 22px; font-weight: 700; color: #ffffff; margin: 0 0 12px 0; letter-spacing: -0.3px;"">
                  ¡Estás a un paso de empezar! 🔥
                </h1>
                <p style=""color: #a0a0a0; font-size: 15px; line-height: 1.6; margin: 0 0 28px 0;"">
                  Gracias por unirte a <strong style=""color: #ffffff;"">PyrosFit</strong>. Para activar tu perfil y acceder a la plataforma, confirma tu dirección de correo presionando el botón de abajo.
                </p>
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                  <tr>
                    <td align=""center"" style=""padding: 8px 0 24px 0;"">
                      <a href=""{link}"" target=""_blank"" style=""display: inline-block; background: linear-gradient(135deg, #ff6b00 0%, #e64a00 100%); color: #ffffff; font-size: 15px; font-weight: 800; text-decoration: none; padding: 16px 36px; border-radius: 10px; text-transform: uppercase; letter-spacing: 1px; box-shadow: 0 6px 20px rgba(255, 107, 0, 0.35);"">
                        ACTIVAR MI CUENTA &rarr;
                      </a>
                    </td>
                  </tr>
                </table>
                <div style=""background-color: #171717; border-left: 3px solid #ff5500; border-radius: 4px; padding: 12px 16px; margin-top: 12px;"">
                  <p style=""color: #888888; font-size: 12px; line-height: 1.5; margin: 0;"">
                    ⏱️ Este enlace es personal y expirará en <strong style=""color: #cccccc;"">24 horas</strong>. Si no creaste esta cuenta, puedes ignorar este mensaje.
                  </p>
                </div>
              </td>
            </tr>
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

        private string BuildPasswordResetHtml(string resetLink, string? userName)
        {
            var greeting = string.IsNullOrWhiteSpace(userName) ? "Hola" : $"Hola, {userName}";

            return $@"<!doctype html>
<html lang=""es"">
  <head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Restablecer contraseña - PyrosFit</title>
  </head>
  <body style=""margin: 0; padding: 0; background-color: #080808; font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; -webkit-font-smoothing: antialiased;"">
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""background-color: #080808; padding: 40px 10px;"">
      <tr>
        <td align=""center"">
          <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""max-width: 540px; background-color: #121212; border: 1px solid #222222; border-top: 4px solid #ff5500; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 30px rgba(255, 85, 0, 0.08);"">
            <tr>
              <td style=""padding: 36px 36px 20px 36px;"">
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                  <tr>
                    <td>
                      <span style=""font-size: 24px; font-weight: 900; color: #ffffff; letter-spacing: -0.5px; text-transform: uppercase;"">
                        PYROS<span style=""color: #ff5500;"">FIT</span>
                      </span>
                      <div style=""font-size: 11px; color: #ff5500; font-weight: 700; text-transform: uppercase; letter-spacing: 1.5px; margin-top: 2px;"">
                        Recuperación de Contraseña
                      </div>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td style=""padding: 0 36px;"">
                <div style=""border-bottom: 1px solid #1f1f1f; width: 100%;""></div>
              </td>
            </tr>
            <tr>
              <td style=""padding: 28px 36px 36px 36px;"">
                <h1 style=""font-size: 22px; font-weight: 700; color: #ffffff; margin: 0 0 12px 0; letter-spacing: -0.3px;"">
                  {greeting} 🔐
                </h1>
                <p style=""color: #a0a0a0; font-size: 15px; line-height: 1.6; margin: 0 0 28px 0;"">
                  Recibimos una solicitud para restablecer la contraseña de tu cuenta en <strong style=""color: #ffffff;"">PyrosFit</strong>. Presiona el botón de abajo para establecer una nueva clave segura.
                </p>
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                  <tr>
                    <td align=""center"" style=""padding: 8px 0 24px 0;"">
                      <a href=""{resetLink}"" target=""_blank"" style=""display: inline-block; background: linear-gradient(135deg, #ff6b00 0%, #e64a00 100%); color: #ffffff; font-size: 15px; font-weight: 800; text-decoration: none; padding: 16px 36px; border-radius: 10px; text-transform: uppercase; letter-spacing: 1px; box-shadow: 0 6px 20px rgba(255, 107, 0, 0.35);"">
                        RESTABLECER CONTRASEÑA &rarr;
                      </a>
                    </td>
                  </tr>
                </table>
                <div style=""background-color: #171717; border-left: 3px solid #ff5500; border-radius: 4px; padding: 12px 16px; margin-top: 12px;"">
                  <p style=""color: #888888; font-size: 12px; line-height: 1.5; margin: 0;"">
                    ⏱️ Este enlace expirará en <strong style=""color: #cccccc;"">2 horas</strong> por motivos de seguridad. Si no solicitaste este cambio, puedes ignorar este correo; tu contraseña actual permanecerá intacta.
                  </p>
                </div>
              </td>
            </tr>
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

        private string BuildMotivationHtml(string studentName, string message, string? coachName)
        {
            var coachLabel = string.IsNullOrWhiteSpace(coachName) ? "Tu Entrenador" : coachName;

            return $@"<!doctype html>
<html lang=""es"">
  <head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Mensaje de tu entrenador - PyrosFit</title>
  </head>
  <body style=""margin: 0; padding: 0; background-color: #080808; font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; -webkit-font-smoothing: antialiased;"">
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""background-color: #080808; padding: 40px 10px;"">
      <tr>
        <td align=""center"">
          <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""max-width: 540px; background-color: #121212; border: 1px solid #222222; border-top: 4px solid #ff5500; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 30px rgba(255, 85, 0, 0.08);"">
            <tr>
              <td style=""padding: 36px 36px 20px 36px;"">
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                  <tr>
                    <td>
                      <span style=""font-size: 24px; font-weight: 900; color: #ffffff; letter-spacing: -0.5px; text-transform: uppercase;"">
                        PYROS<span style=""color: #ff5500;"">FIT</span>
                      </span>
                      <div style=""font-size: 11px; color: #ff5500; font-weight: 700; text-transform: uppercase; letter-spacing: 1.5px; margin-top: 2px;"">
                        Radar de Progreso &amp; Motivación
                      </div>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td style=""padding: 0 36px;"">
                <div style=""border-bottom: 1px solid #1f1f1f; width: 100%;""></div>
              </td>
            </tr>
            <tr>
              <td style=""padding: 28px 36px 36px 36px;"">
                <h1 style=""font-size: 22px; font-weight: 700; color: #ffffff; margin: 0 0 12px 0; letter-spacing: -0.3px;"">
                  ¡Hola, {studentName}! 💪🔥
                </h1>
                <p style=""color: #a0a0a0; font-size: 15px; line-height: 1.6; margin: 0 0 20px 0;"">
                  <strong style=""color: #ff5500;"">{coachLabel}</strong> te ha dejado un mensaje especial para mantener el ritmo y alcanzar tus metas:
                </p>
                
                <!-- Tarjeta de Mensaje del Coach -->
                <div style=""background-color: #181818; border-left: 4px solid #ff6b00; border-radius: 8px; padding: 20px; margin: 0 0 28px 0; box-shadow: inset 0 2px 4px rgba(0,0,0,0.4);"">
                  <p style=""color: #ffffff; font-size: 16px; font-style: italic; line-height: 1.7; margin: 0; white-space: pre-line;"">
                    &ldquo;{message}&rdquo;
                  </p>
                </div>

                <div style=""background-color: #171717; border-radius: 8px; padding: 14px 18px; margin-bottom: 24px;"">
                  <p style=""color: #cccccc; font-size: 13px; line-height: 1.5; margin: 0;"">
                    ⚡ <strong>¡No dejes que tu racha caiga!</strong> Cada entrenamiento cuenta para construir la mejor versión de ti mismo.
                  </p>
                </div>
              </td>
            </tr>
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
