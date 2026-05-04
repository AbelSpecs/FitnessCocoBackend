using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Commands.DeleteStudentCommand
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _unitOfWork.Students.DeleteAsync(request.Id);
                if (!deleted) return new Response<bool> { IsSuccess = false, Message = "Student not found or could not be deleted.", Data = false };
                return new Response<bool> { IsSuccess = true, Data = true, Message = "Student deleted." };
            }
            catch (Exception ex)
            {
                return new Response<bool> { IsSuccess = false, Message = ex.Message, Data = false };
            }
        }
    }
}
