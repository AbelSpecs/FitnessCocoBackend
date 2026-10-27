using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Presentation
{
    public interface IFirebaseNotificationService
    {
        Task<bool> SendPushToUserAsync(int userId, string title, string body, Dictionary<string, string>? data = null, CancellationToken ct = default);
    }
}