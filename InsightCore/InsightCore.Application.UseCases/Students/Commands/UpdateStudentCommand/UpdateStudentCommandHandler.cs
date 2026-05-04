using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Commands.UpdateStudentCommand
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Response<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<StudentDto>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _unitOfWork.Students.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    return new Response<StudentDto> { IsSuccess = false, Message = "Student not found." };
                }

                // Map fields
                _mapper.Map(request.Student, existing);
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _unitOfWork.Students.UpdateAsync(existing);
                if (!updated) return new Response<StudentDto> { IsSuccess = false, Message = "Could not update student." };

                var dto = _mapper.Map<StudentDto>(existing);
                return new Response<StudentDto> { IsSuccess = true, Data = dto, Message = "Student updated." };
            }
            catch (Exception ex)
            {
                return new Response<StudentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
