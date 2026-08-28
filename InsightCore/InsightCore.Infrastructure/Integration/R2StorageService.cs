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
            var creds = new BasicAWSCredentials(_settings.AccessKeyId, _settings.SecretAccessKey);
            var config = new AmazonS3Config
            {
                ServiceURL = _urlStorage,
                ForcePathStyle = true,
            };

            if (!string.IsNullOrEmpty(_settings.Region))
            {
                try
                {
                    // If region provided, set it; otherwise rely on ServiceURL
                    config.RegionEndpoint = RegionEndpoint.USEast1;
                }
                catch { /* ignore */ }
            }

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
