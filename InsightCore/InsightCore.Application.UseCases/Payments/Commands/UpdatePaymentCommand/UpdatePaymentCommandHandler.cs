using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Payments.Commands.UpdatePaymentCommand
{
    /// <summary>
    /// Maneja la modificación de un pago existente.
    /// El caso de uso más común es cambiar el Status de Pendiente → Aprobado/Rechazado.
    /// </summary>
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Response<PaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePaymentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PaymentDto>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _unitOfWork.Payments.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    return new Response<PaymentDto>
                    {
                        IsSuccess = false,
                        Message = $"No se encontró el pago con Id {request.Id}."
                    };
                }

                // Mapear los campos del DTO sobre la entidad existente
                _mapper.Map(request.Payment, existing);
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _unitOfWork.Payments.UpdateAsync(existing);
                if (!updated)
                {
                    return new Response<PaymentDto>
                    {
                        IsSuccess = false,
                        Message = "No se pudo actualizar el pago."
                    };
                }

                var dto = _mapper.Map<PaymentDto>(existing);
                return new Response<PaymentDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = "Pago actualizado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new Response<PaymentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
