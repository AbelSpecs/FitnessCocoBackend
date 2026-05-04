using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Commands.CreateStudentCommand
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Response<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<StudentDto>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var student = _mapper.Map<Student>(request.Student);
                student.CreatedAt = DateTime.UtcNow;

                var created = await _unitOfWork.Students.InsertAsync(student);
                var dto = _mapper.Map<StudentDto>(created);

                return new Response<StudentDto> { IsSuccess = true, Data = dto, Message = "Student created." };
            }
            catch (Exception ex)
            {
                return new Response<StudentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
