using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Notify = FirebaseAdmin.Messaging.Notification;
using Google.Apis.Auth.OAuth2;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InsightCore.Infrastructure.Notification.Services
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FirebaseNotificationService> _logger;

        public FirebaseNotificationService(
            ApplicationDbContext context,
            ILogger<FirebaseNotificationService> logger,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;

            EnsureFirebaseInitialized(configuration);
        }

        private static void EnsureFirebaseInitialized(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance != null) return;

            var keyPath = configuration["Firebase:CredentialsPath"] ?? "secrets/pyrosfit-firebase-adminsdk.json";

            if (File.Exists(keyPath))
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(keyPath)
                });
            }
        }

        public async Task<bool> SendPushToUserAsync(int userId, string title, string body, Dictionary<string, string>? data = null, CancellationToken ct = default)
        {
            var tokens = await _context.UserDeviceTokens
                .Where(t => t.UserId == userId && t.IsActive)
                .Select(t => t.FcmToken)
                .ToListAsync(ct);

            if (!tokens.Any()) return false;

            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notify { Title = title, Body = body },
                Data = data ?? new Dictionary<string, string>(),
                Android = new AndroidConfig { Priority = Priority.High },
                Apns = new ApnsConfig { Aps = new Aps { Sound = "default" } }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message, ct);
            return response.SuccessCount > 0;
        }
    }
}
