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
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IMapper _mapper;

        public AssignDailyExerciseCommandHandler(IDailyStudentExercisesRepository dailyRepo, IExercisesRepository exercisesRepository, IMapper mapper)
        {
            _dailyRepo = dailyRepo;
            _exercisesRepository = exercisesRepository;
            _mapper = mapper;
        }

        public async Task<Response<AssignDailyExerciseDto>> Handle(AssignDailyExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<AssignDailyExerciseDto>();
            try
            {
                // Basic validation: ExerciseId must be provided and must exist
                if (request.Assign == null || request.Assign.ExerciseId <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "ExerciseId is required.";
                    return response;
                }

                var existingExercise = await _exercisesRepository.GetByIdAsync(request.Assign.ExerciseId);
                if (existingExercise == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Exercise not found.";
                    return response;
                }

                var entity = _mapper.Map<DailyStudentExercise>(request.Assign);
                // Ensure we do not accidentally attempt to insert or update the Exercise navigation
                // Only ExerciseId should be used to reference the existing exercise
                entity.Exercise = null;
                // Ensure child sets are initialized and mapped
                if (request.Assign.DailyExerciseSets != null)
                {
                    var sets = _mapper.Map<IEnumerable<DailyExerciseSet>>(request.Assign.DailyExerciseSets).ToList();
                    // Ensure children do not carry an explicit FK value and link them to parent entity
                    foreach (var s in sets)
                    {
                        s.DailyStudentExercise = entity;
                        s.DailyStudentExerciseId = 0; // let EF set FK when saving parent
                        // ensure no accidental navigation to Exercise on set level
                        // (DailyExerciseSet doesn't have Exercise navigation, but guard if present in future)
                        // s.Exercise = null;
                    }
                    entity.DailyExerciseSets = sets;
                }

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
