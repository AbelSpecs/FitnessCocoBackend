namespace InsightCore.Application.DTO.Payments
{
    public record PayPalCreateOrderRequest(decimal Amount, string Currency, string Description, string ReturnUrl, string CancelUrl);

    public record PayPalCreateOrderResult(string OrderId, string ApprovalUrl);

    public record PayPalCaptureOrderResult(string OrderId, string Status, string? CaptureId);
}
