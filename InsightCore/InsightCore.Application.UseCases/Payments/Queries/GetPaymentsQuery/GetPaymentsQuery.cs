using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;

namespace InsightCore.Application.UseCases.Payments.Queries.GetPaymentsQuery
{
    /// <summary>
    /// Consulta paginada de pagos con filtros opcionales por usuario,
    /// alumno, moneda, estado y rango de fechas.
    /// </summary>
    public class GetPaymentsQuery : IRequest<Response<PagedResult<PaymentDto>>>
    {
        /// <summary>Filtrar por UserId (opcional).</summary>
        public int? UserId { get; set; }

        /// <summary>Filtrar por StudentId (opcional).</summary>
        public int? StudentId { get; set; }

        /// <summary>
        /// Filtrar por moneda como string: "VES", "USD" o "USDT" (opcional).
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Filtrar por estado como string: "Pendiente", "Aprobado" o "Rechazado" (opcional).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>Fecha de inicio del rango (opcional).</summary>
        public DateTime? From { get; set; }

        /// <summary>Fecha de fin del rango (opcional).</summary>
        public DateTime? To { get; set; }

        /// <summary>Número de página base-1. Por defecto: 1.</summary>
        public int Page { get; set; } = 1;

        /// <summary>Tamaño de página. Por defecto: 20. Máximo: 100.</summary>
        public int PageSize { get; set; } = 20;
    }
}
