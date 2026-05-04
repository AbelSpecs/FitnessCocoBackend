using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.AssignDailyExerciseCommand
{
    public class AssignDailyExerciseCommandHandler : IRequestHandler<AssignDailyExerciseCommand, Response<AssignDailyExerciseDto>>
    {
        private readonly IDailyStudentExercisesRepository _dailyRepo;
        private readonly IMapper _mapper;

        public AssignDailyExerciseCommandHandler(IDailyStudentExercisesRepository dailyRepo, IMapper mapper)
        {
            _dailyRepo = dailyRepo;
            _mapper = mapper;
        }

        public async Task<Response<AssignDailyExerciseDto>> Handle(AssignDailyExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<AssignDailyExerciseDto>();
            try
            {
                var entity = _mapper.Map<DailyStudentExercise>(request.Assign);
                var created = await _dailyRepo.InsertAsync(entity);
                response.Data = _mapper.Map<AssignDailyExerciseDto>(created);
                response.IsSuccess = true;
                response.Message = "Assigned successfully.";
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
