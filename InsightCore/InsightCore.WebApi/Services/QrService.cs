using InsightCore.Application.Interface.Presentation;
using QRCoder;

namespace InsightCore.WebApi.Services
{
    public class QrService : IQrService
    {
        public string GenerateQrBase64(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrBytes = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrBytes);
        }
    }
}
