namespace InsightCore.Domain.Enums
{
    /// <summary>
    /// Métodos de pago aceptados por el sistema Pyrosfit.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>Transferencia bancaria nacional</summary>
        Transferencia = 1,

        /// <summary>Pago Móvil (interoperabilidad bancaria venezolana)</summary>
        PagoMovil = 2,

        /// <summary>Zelle (transferencia USD)</summary>
        Zelle = 3,

        /// <summary>Binance Pay (cripto USDT)</summary>
        BinancePay = 4,

        /// <summary>Efectivo en persona</summary>
        Efectivo = 5,

        /// <summary>PayPal</summary>
        PayPal = 6,

        /// <summary>Otro método no listado</summary>
        Otro = 99
    }
}
