using System;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Integration
{
    public interface IStorageService
    {
        Task<string> GeneratePresignedUploadUrl(string fileKey, string contentType, TimeSpan expiration);
        Task<string> GeneratePresignedDownloadUrl(string fileKey, TimeSpan expiration);
    }
}
