using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Presentation
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string confirmationLink);
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink, string? userName = null);
        Task SendPasswordChangedNotificationEmailAsync(string toEmail, string? userName = null);
        Task SendMotivationEmailAsync(string toEmail, string studentName, string message, string? coachName = null);
    }
}
