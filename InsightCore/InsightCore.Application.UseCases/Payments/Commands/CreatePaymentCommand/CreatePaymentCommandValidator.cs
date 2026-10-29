using FluentValidation;
using InsightCore.Domain.Enums;

namespace InsightCore.Application.UseCases.Payments.Commands.CreatePaymentCommand
{
    /// <summary>
    /// Validaciones de negocio para el registro de un nuevo pago.
    /// Garantiza coherencia entre moneda, método y referencia.
    /// El DTO usa ints que mapean al enum de dominio:
    ///   Currency  → PaymentCurrency  (1=VES, 2=USD, 3=USDT)
    ///   Method    → PaymentMethod    (1=Transferencia, 2=PagoMovil, 3=Zelle, 4=BinancePay, 5=Efectivo, 6=PayPal, 99=Otro)
    ///   Status    → PaymentStatus    (1=Pendiente, 2=Aprobado, 3=Rechazado)
    /// </summary>
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        // Valores del enum como constantes locales para mayor legibilidad
        private const int CurrencyVES   = (int)PaymentCurrency.VES;
        private const int CurrencyUSD   = (int)PaymentCurrency.USD;
        private const int CurrencyUSDT  = (int)PaymentCurrency.USDT;
        private const int MethodPagoMovil  = (int)PaymentMethod.PagoMovil;
        private const int MethodBinancePay = (int)PaymentMethod.BinancePay;
        private const int MethodEfectivo   = (int)PaymentMethod.Efectivo;
        private const int MethodOtro       = (int)PaymentMethod.Otro;

        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.Payment)
                .NotNull().WithMessage("Los datos del pago son requeridos.");

            RuleFor(x => x.Payment.UserId)
                .GreaterThan(0).WithMessage("UserId debe ser un entero positivo.");

            RuleFor(x => x.Payment.Amount)
                .GreaterThan(0m).WithMessage("El monto debe ser mayor a cero.")
                .GreaterThanOrEqualTo(0.0001m).WithMessage("El monto mínimo permitido es 0.0001.");

            // Referencia obligatoria para pagos no-efectivo
            RuleFor(x => x.Payment.Reference)
                .NotEmpty()
                .When(x => x.Payment.PaymentMethod != MethodEfectivo)
                .WithMessage("La referencia o hash es obligatoria para el método de pago seleccionado.");

            // Para USDT/Binance Pay la referencia debe parecer un hash (>= 20 chars)
            RuleFor(x => x.Payment.Reference)
                .MinimumLength(20)
                .When(x => x.Payment.Currency == CurrencyUSDT
                         || x.Payment.PaymentMethod == MethodBinancePay)
                .WithMessage("Para USDT / Binance Pay la referencia (hash) debe tener al menos 20 caracteres.");

            // Pago Móvil es exclusivo de VES
            RuleFor(x => x.Payment.PaymentMethod)
                .NotEqual(MethodPagoMovil)
                .When(x => x.Payment.Currency != CurrencyVES)
                .WithMessage("El método 'Pago Móvil' sólo es válido para pagos en Bolívares (VES).");

            // USDT solo acepta BinancePay u Otro
            RuleFor(x => x.Payment.PaymentMethod)
                .Must(m => m == MethodBinancePay || m == MethodOtro)
                .When(x => x.Payment.Currency == CurrencyUSDT)
                .WithMessage("Los pagos en USDT solo aceptan BinancePay u Otro como método.");

            // Validación de dominio de enums (los enteros deben ser valores válidos)
            RuleFor(x => x.Payment.Currency)
                .Must(c => Enum.IsDefined(typeof(PaymentCurrency), c))
                .WithMessage("Moneda inválida. Use: 1=VES, 2=USD, 3=USDT.");

            RuleFor(x => x.Payment.PaymentMethod)
                .Must(m => Enum.IsDefined(typeof(PaymentMethod), m))
                .WithMessage("Método de pago inválido.");

            RuleFor(x => x.Payment.Status)
                .Must(s => Enum.IsDefined(typeof(PaymentStatus), s))
                .WithMessage("Estado de pago inválido. Use: 1=Pendiente, 2=Aprobado, 3=Rechazado.");
        }
    }
}
