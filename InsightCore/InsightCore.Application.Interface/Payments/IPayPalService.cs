using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO.Payments;

namespace InsightCore.Application.Interface.Payments
{
    public interface IPayPalService
    {
        Task<PayPalCreateOrderResult?> CreateOrderAsync(PayPalCreateOrderRequest request, CancellationToken cancellationToken);
        Task<PayPalCaptureOrderResult?> CaptureOrderAsync(string orderId, CancellationToken cancellationToken);
    }
}
