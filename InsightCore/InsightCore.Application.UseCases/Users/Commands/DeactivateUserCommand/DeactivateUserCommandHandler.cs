using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Users.Commands.DeactivateUserCommand
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
                if (user is null)
                {
                    return new Response<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "Usuario no encontrado."
                    };
                }

                user.Status = false;
                user.DeletedDate = DateTime.UtcNow;
                user.LastModified = DateTime.UtcNow;
                user.LastModifiedBy = "System";

                var updated = await _unitOfWork.Users.UpdateAsync(user);
                if (!updated)
                {
                    return new Response<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "No se pudo dar de baja al usuario."
                    };
                }

                return new Response<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Usuario dado de baja correctamente."
                };
            }
            catch (Exception ex)
            {
                return new Response<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = ex.Message
                };
            }
        }
    }
}
