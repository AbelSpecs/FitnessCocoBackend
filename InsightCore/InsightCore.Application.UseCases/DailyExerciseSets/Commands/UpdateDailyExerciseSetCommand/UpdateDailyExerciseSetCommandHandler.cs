using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.UpdateDailyExerciseSetCommand
{
    public class UpdateDailyExerciseSetCommandHandler : IRequestHandler<UpdateDailyExerciseSetCommand, Response<DailyExerciseSetDto>>
    {
        private readonly IDailyExerciseSetsRepository _repository;
        private readonly IDailyStudentExercisesRepository _dailyStudentExercisesRepository;
        private readonly IMapper _mapper;

        public UpdateDailyExerciseSetCommandHandler(IDailyExerciseSetsRepository repository, IDailyStudentExercisesRepository dailyStudentExercisesRepository, IMapper mapper)
        {
            _repository = repository;
            _dailyStudentExercisesRepository = dailyStudentExercisesRepository;
            _mapper = mapper;
        }

        public async Task<Response<DailyExerciseSetDto>> Handle(UpdateDailyExerciseSetCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<DailyExerciseSetDto>();
            try
            {
                var existing = await _repository.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Set not found.";
                    return response;
                }

                // Validate parent
                var parent = await _dailyStudentExercisesRepository.GetByIdAsync(request.Set.DailyStudentExerciseId);
                if (parent == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Daily student exercise not found.";
                    return response;
                }

                if (request.Set.SetNumber <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "SetNumber must be greater than zero.";
                    return response;
                }

                if (request.Set.TargetReps <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "TargetReps must be greater than zero.";
                    return response;
                }

                if (request.Set.TargetWeight < 0)
                {
                    response.IsSuccess = false;
                    response.Message = "TargetWeight must be zero or positive.";
                    return response;
                }

                _mapper.Map(request.Set, existing);
                existing.Id = request.Id;
                var updated = await _repository.UpdateAsync(existing);
                response.IsSuccess = updated;
                response.Data = _mapper.Map<DailyExerciseSetDto>(existing);
                response.Message = updated ? "Set updated." : "No changes made.";
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
