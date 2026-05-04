using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentQuery
{
    public class GetDailyExercisesByStudentQueryHandler : IRequestHandler<GetDailyExercisesByStudentQuery, Response<IEnumerable<AssignDailyExerciseDto>>>
    {
        private readonly IDailyStudentExercisesRepository _dailyRepo;
        private readonly IMapper _mapper;

        public GetDailyExercisesByStudentQueryHandler(IDailyStudentExercisesRepository dailyRepo, IMapper mapper)
        {
            _dailyRepo = dailyRepo;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AssignDailyExerciseDto>>> Handle(GetDailyExercisesByStudentQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<AssignDailyExerciseDto>>();
            try
            {
                var list = await _dailyRepo.GetByStudentAsync(request.StudentId);
                response.Data = _mapper.Map<IEnumerable<AssignDailyExerciseDto>>(list);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
