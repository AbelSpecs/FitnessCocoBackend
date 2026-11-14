using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Payments.Queries.GetPaymentByIdQuery
{
    /// <summary>
    /// Obtiene el detalle completo de un pago por su Id.
    /// </summary>
    public class GetPaymentByIdQuery : IRequest<Response<PaymentDto>>
    {
        public int Id { get; set; }
    }
}
