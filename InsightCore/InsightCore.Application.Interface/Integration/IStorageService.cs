using System;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Integration
{
    public interface IStorageService
    {
        Task<string> GeneratePresignedUploadUrl(string fileKey, string contentType, TimeSpan expiration);
        Task<string> GeneratePresignedDownloadUrl(string fileKey, TimeSpan expiration);
        /// <summary>
        /// Obtiene el stream del objeto almacenado en el bucket junto con su content-type y content-length.
        /// El stream devuelto debe ser dispuesto por el consumidor para liberar recursos subyacentes.
        /// </summary>
        Task<(System.IO.Stream Stream, string? ContentType, long? ContentLength)> GetObjectStreamAsync(string fileKey);
    }
}
