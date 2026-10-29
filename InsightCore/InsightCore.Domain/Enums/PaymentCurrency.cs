namespace InsightCore.Domain.Enums
{
    /// <summary>
    /// Monedas soportadas para los pagos en el sistema Pyrosfit.
    /// </summary>
    public enum PaymentCurrency
    {
        /// <summary>Bolívar Soberano (Venezuela)</summary>
        VES = 1,

        /// <summary>Dólar Estadounidense</summary>
        USD = 2,

        /// <summary>Tether USD (stablecoin cripto)</summary>
        USDT = 3
    }
}
