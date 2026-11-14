namespace InsightCore.Domain.Enums
{
    /// <summary>
    /// Estado del ciclo de vida de un pago.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>Pago registrado, pendiente de verificación</summary>
        Pendiente = 1,

        /// <summary>Pago verificado y aprobado por el coach/administrador</summary>
        Aprobado = 2,

        /// <summary>Pago rechazado (referencia inválida, monto incorrecto, etc.)</summary>
        Rechazado = 3
    }
}
