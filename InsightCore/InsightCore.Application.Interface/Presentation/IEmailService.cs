using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Presentation
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string confirmationLink);
    }
}
