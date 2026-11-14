using System;

namespace InsightCore.Application.DTO
{
    /// <summary>
    /// DTO de lectura para un pago registrado.
    /// Los campos de enumeración se exponen como strings legibles.
    /// </summary>
    public class PaymentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public int? StudentId { get; set; }
        public decimal Amount { get; set; }

        /// <summary>Moneda como string: "VES", "USD" o "USDT".</summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>Método de pago como string legible (ej. "Transferencia", "Zelle").</summary>
        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }
        public string? Reference { get; set; }

        /// <summary>Estado como string: "Pendiente", "Aprobado" o "Rechazado".</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// URL pública del comprobante generada en tiempo de consulta
        /// a partir de la ReceiptKey almacenada en R2.
        /// </summary>
        public string? ReceiptUrl { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO de escritura para registrar o actualizar un pago.
    /// Usa enteros que mapean directamente a los valores del enum en el dominio.
    /// </summary>
    public class CreateOrUpdatePaymentDto
    {
        public int UserId { get; set; }
        //public int? StudentId { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// Moneda del pago como entero (enum PaymentCurrency):
        /// 1 = VES (Bolívares), 2 = USD, 3 = USDT.
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// Método de pago como entero (enum PaymentMethod):
        /// 1 = Transferencia, 2 = PagoMovil, 3 = Zelle,
        /// 4 = BinancePay, 5 = Efectivo, 6 = PayPal, 99 = Otro.
        /// </summary>
        public int PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        /// <summary>Número de referencia bancaria o hash de transacción cripto.</summary>
        public string? Reference { get; set; }

        /// <summary>
        /// Estado del pago como entero (enum PaymentStatus):
        /// 1 = Pendiente, 2 = Aprobado, 3 = Rechazado. Por defecto: 1.
        /// </summary>
        public int Status { get; set; } = 1; // PaymentStatus.Pendiente

        /// <summary>
        /// Key del comprobante en Cloudflare R2 (solo la key, nunca la URL).
        /// </summary>
        public string? ReceiptKey { get; set; }

        public string? Notes { get; set; }
    }

    /// <summary>
    /// Parámetros de filtro / paginación para la consulta de pagos.
    /// Enteros opcionales que mapean a los valores del enum de dominio.
    /// </summary>
    public class PaymentFilterDto
    {
        public int? UserId { get; set; }
        //public int? StudentId { get; set; }

        /// <summary>1=VES, 2=USD, 3=USDT</summary>
        public int? Currency { get; set; }

        /// <summary>1=Pendiente, 2=Aprobado, 3=Rechazado</summary>
        public int? Status { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        /// <summary>Número de página base-1. Por defecto: 1.</summary>
        public int Page { get; set; } = 1;

        /// <summary>Tamaño de página. Por defecto: 20. Máximo: 100.</summary>
        public int PageSize { get; set; } = 20;
    }
}
