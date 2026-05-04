using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.CoachStudents.Queries.GetCoachStudentQuery
{
    public class GetCoachStudentQueryHandler : IRequestHandler<GetCoachStudentQuery, Response<CoachStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCoachStudentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CoachStudentDto>> Handle(GetCoachStudentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
                if (student == null) return new Response<CoachStudentDto> { IsSuccess = false, Message = "Student not found." };

                var relation = await _unitOfWork.CoachStudents.GetByIdsAsync(request.CoachId, request.StudentId);
                if (relation == null) return new Response<CoachStudentDto> { IsSuccess = false, Message = "Relation not found." };

                var dto = _mapper.Map<CoachStudentDto>(relation);
                return new Response<CoachStudentDto> { IsSuccess = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new Response<CoachStudentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
