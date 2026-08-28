using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using InsightCore.Application.Interface.Integration;
using InsightCore.Infrastructure.Notification;
using InsightCore.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RTools_NTS.Util;
using System;
using System.Threading.Tasks;

namespace InsightCore.Infrastructure.Integration
{
    public class R2StorageService : IStorageService
    {
        private readonly R2Settings _settings;
        private readonly ILogger<R2StorageService> _logger;

        private readonly IConfiguration _configuration;
        private readonly string _urlStorage;
        //private readonly string _AccessKeyId;
        //private readonly string _SecretAccessKey;

        public R2StorageService(IOptions<R2Settings> options,IConfiguration configuration, ILogger<R2StorageService> logger)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _configuration = configuration;
            _logger = logger;
            //_urlStorage = configuration["R2Settings:ServiceUrl"] ?? "";
            //_Token = configuration["R2Settings:AccessKeyId"] ?? "";
            //_SecretAccessKey = configuration["R2Settings:SecretAccessKey"] ?? "";
        }

        private AmazonS3Client CreateClient()
        {
            // Resolve service URL from settings first, then fallback to configured _urlStorage.
            // Validate early so callers get a clear error if no endpoint is configured.
            var serviceUrl = !string.IsNullOrWhiteSpace(_settings.ServiceUrl)
                ? _settings.ServiceUrl
                : _urlStorage;

            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                _logger.LogError("R2Settings.ServiceUrl is not configured. Please set R2Settings.ServiceUrl to your Cloudflare R2 account endpoint.");
                throw new InvalidOperationException("R2Settings.ServiceUrl must be configured and point to your Cloudflare R2 account endpoint.");
            }

            var creds = new BasicAWSCredentials(_settings.AccessKeyId, _settings.SecretAccessKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = serviceUrl,
                SignatureVersion = "4",
                ForcePathStyle = true
            };

            // Forzar uso de Signature Version 4 en caso de que la configuración por defecto use V2
            try
            {
                Amazon.AWSConfigsS3.UseSignatureVersion4 = true;
            }
            catch { }

            return new AmazonS3Client(creds, config);
        }

        public Task<string> GeneratePresignedDownloadUrl(string fileKey, TimeSpan expiration)
        {
            try
            {
                using var client = CreateClient();
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _settings.BucketName,
                    Key = fileKey,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.Add(expiration)
                };

                var url = client.GetPreSignedURL(request);
                return Task.FromResult(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating presigned download url for {Key}", fileKey);
                throw;
            }
        }

        public Task<string> GeneratePresignedUploadUrl(string fileKey, string contentType, TimeSpan expiration)
        {
            try
            {
                using var client = CreateClient();
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _settings.BucketName,
                    Key = fileKey,
                    Verb = HttpVerb.PUT,
                    Expires = DateTime.UtcNow.Add(expiration),
                    ContentType = contentType
                };

                var url = client.GetPreSignedURL(request);
                return Task.FromResult(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating presigned upload url for {Key}", fileKey);
                throw;
            }
        }
    }
}
