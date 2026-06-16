using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.UpdateDailyExerciseCommand
{
    public class UpdateDailyExerciseCommandHandler : IRequestHandler<UpdateDailyExerciseCommand, Response<AssignDailyExerciseDto>>
    {
        private readonly IDailyStudentExercisesRepository _dailyRepo;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IMapper _mapper;

        public UpdateDailyExerciseCommandHandler(IDailyStudentExercisesRepository dailyRepo, IExercisesRepository exercisesRepository, IMapper mapper)
        {
            _dailyRepo = dailyRepo;
            _exercisesRepository = exercisesRepository;
            _mapper = mapper;
        }

        public async Task<Response<AssignDailyExerciseDto>> Handle(UpdateDailyExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<AssignDailyExerciseDto>();
            try
            {
                var existing = await _dailyRepo.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Message = "DailyStudentExercise not found.";
                    return response;
                }

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

                // Map incoming fields onto the entity
                _mapper.Map(request.Assign, existing);

                // Ensure navigation property not accidentally overwritten
                existing.Exercise = existingExercise;

                // Handle child sets: for simplicity follow same pattern as Assign handler
                if (request.Assign.DailyExerciseSets != null)
                {
                    // replace child collection with mapped children
                    var sets = _mapper.Map<IEnumerable<DailyExerciseSet>>(request.Assign.DailyExerciseSets).ToList();
                    foreach (var s in sets)
                    {
                        s.DailyStudentExercise = existing;
                        s.DailyStudentExerciseId = existing.Id;
                    }
                    existing.DailyExerciseSets = sets;
                }

                var updated = await _dailyRepo.UpdateAsync(existing);
                response.IsSuccess = updated;
                response.Data = _mapper.Map<AssignDailyExerciseDto>(existing);
                response.Message = updated ? "Daily exercise updated." : "No changes made.";
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
