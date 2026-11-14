using FluentValidation;
using InsightCore.Domain.Enums;

namespace InsightCore.Application.UseCases.Payments.Commands.UpdatePaymentCommand
{
    /// <summary>
    /// Validaciones para la actualización de un pago existente.
    /// Reutiliza las mismas reglas de coherencia moneda/método que el Create.
    /// </summary>
    public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
    {
        private const int CurrencyVES      = (int)PaymentCurrency.VES;
        private const int CurrencyUSDT     = (int)PaymentCurrency.USDT;
        private const int MethodPagoMovil  = (int)PaymentMethod.PagoMovil;
        private const int MethodBinancePay = (int)PaymentMethod.BinancePay;
        private const int MethodEfectivo   = (int)PaymentMethod.Efectivo;
        private const int MethodOtro       = (int)PaymentMethod.Otro;

        public UpdatePaymentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id del pago inválido.");

            RuleFor(x => x.Payment)
                .NotNull().WithMessage("Los datos del pago son requeridos.");

            RuleFor(x => x.Payment.Amount)
                .GreaterThan(0m).WithMessage("El monto debe ser mayor a cero.");

            RuleFor(x => x.Payment.Reference)
                .NotEmpty()
                .When(x => x.Payment.PaymentMethod != MethodEfectivo)
                .WithMessage("La referencia o hash es obligatoria para el método de pago seleccionado.");

            RuleFor(x => x.Payment.Reference)
                .MinimumLength(20)
                .When(x => x.Payment.Currency == CurrencyUSDT
                         || x.Payment.PaymentMethod == MethodBinancePay)
                .WithMessage("Para USDT / Binance Pay la referencia (hash) debe tener al menos 20 caracteres.");

            RuleFor(x => x.Payment.PaymentMethod)
                .NotEqual(MethodPagoMovil)
                .When(x => x.Payment.Currency != CurrencyVES)
                .WithMessage("El método 'Pago Móvil' sólo es válido para pagos en Bolívares (VES).");

            RuleFor(x => x.Payment.Currency)
                .Must(c => Enum.IsDefined(typeof(PaymentCurrency), c))
                .WithMessage("Moneda inválida.");

            RuleFor(x => x.Payment.PaymentMethod)
                .Must(m => Enum.IsDefined(typeof(PaymentMethod), m))
                .WithMessage("Método de pago inválido.");

            RuleFor(x => x.Payment.Status)
                .Must(s => Enum.IsDefined(typeof(PaymentStatus), s))
                .WithMessage("Estado de pago inválido.");
        }
    }
}
