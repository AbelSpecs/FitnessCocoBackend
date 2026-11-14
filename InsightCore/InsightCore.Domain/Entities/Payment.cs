using InsightCore.Domain.Common;
using InsightCore.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    /// <summary>
    /// Representa un pago registrado en el sistema Pyrosfit.
    /// Soporta múltiples monedas (VES, USD, USDT) y métodos de pago
    /// (Transferencia, Pago Móvil, Zelle, Binance Pay, Efectivo, etc.)
    /// manteniendo trazabilidad de referencias y comprobantes.
    /// </summary>
    [Table("Payments")]
    public class Payment : BaseEntity
    {
        /// <summary>
        /// Referencia al usuario (alumno o coach) que realiza el pago.
        /// FK → Users.Id
        /// </summary>
        [Required]
        [Column("UserId")]
        public int UserId { get; set; }

        ///// <summary>
        ///// Referencia al alumno asociado al pago (mensualidad, plan, etc.).
        ///// FK → Students.Id (nullable: puede ser un pago de coach u otro actor)
        ///// </summary>
        //[Column("StudentId")]
        //public int? StudentId { get; set; }

        /// <summary>
        /// Monto del pago con alta precisión decimal (18 enteros, 4 decimales).
        /// Soporta montos grandes en Bolívares y precisión cripto.
        /// </summary>
        [Required]
        [Column("Amount", TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Moneda del pago: VES (Bolívares), USD (Dólares) o USDT (Tether).
        /// </summary>
        [Required]
        [Column("Currency")]
        public PaymentCurrency Currency { get; set; }

        /// <summary>
        /// Método de pago utilizado: Transferencia, Pago Móvil, Zelle, Binance Pay, Efectivo, etc.
        /// </summary>
        [Required]
        [Column("PaymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column("PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Número de referencia bancaria, comprobante o hash de transacción cripto.
        /// Se recomienda único por moneda/método para evitar pagos duplicados.
        /// </summary>
        [MaxLength(255)]
        [Column("Reference")]
        public string? Reference { get; set; }

        /// <summary>
        /// Estado del pago: Pendiente, Aprobado o Rechazado.
        /// </summary>
        [Required]
        [Column("Status")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pendiente;

        /// <summary>
        /// Key del comprobante de pago almacenado en Cloudflare R2 (S3-compatible).
        /// Solo se almacena la key, nunca la URL completa.
        /// </summary>
        [MaxLength(512)]
        [Column("ReceiptKey")]
        public string? ReceiptKey { get; set; }

        /// <summary>
        /// Notas o comentarios adicionales sobre el pago (ej. motivo de rechazo).
        /// </summary>
        [Column("Notes")]
        public string? Notes { get; set; }

        /// <summary>Fecha y hora UTC de creación del registro.</summary>
        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Fecha y hora UTC de la última modificación.</summary>
        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        // ──────────────────────────────────────
        // Navegación (lazy / explícita)
        // ──────────────────────────────────────

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        //[ForeignKey("StudentId")]
        //public virtual Student? Student { get; set; }
    }
}
