using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Queries.GetStudentQuery
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, Response<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<StudentDto>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(request.Id);
                if (student == null)
                {
                    return new Response<StudentDto> { IsSuccess = false, Message = "Student not found." };
                }

                var dto = _mapper.Map<StudentDto>(student);
                return new Response<StudentDto> { IsSuccess = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new Response<StudentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
