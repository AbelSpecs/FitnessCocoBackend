using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.CreateDailyExerciseSetCommand
{
    public class CreateDailyExerciseSetCommandHandler : IRequestHandler<CreateDailyExerciseSetCommand, Response<DailyExerciseSetDto>>
    {
        private readonly IDailyExerciseSetsRepository _repository;
        private readonly IDailyStudentExercisesRepository _dailyStudentExercisesRepository;
        private readonly IMapper _mapper;

        public CreateDailyExerciseSetCommandHandler(IDailyExerciseSetsRepository repository, IDailyStudentExercisesRepository dailyStudentExercisesRepository, IMapper mapper)
        {
            _repository = repository;
            _dailyStudentExercisesRepository = dailyStudentExercisesRepository;
            _mapper = mapper;
        }

        public async Task<Response<DailyExerciseSetDto>> Handle(CreateDailyExerciseSetCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<DailyExerciseSetDto>();
            try
            {
                // Basic validation
                if (request.Set.DailyStudentExerciseId <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid DailyStudentExerciseId.";
                    return response;
                }

                // Verify the parent DailyStudentExercise exists
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

                var entity = _mapper.Map<DailyExerciseSet>(request.Set);
                var created = await _repository.InsertAsync(entity);
                response.Data = _mapper.Map<DailyExerciseSetDto>(created);
                response.IsSuccess = true;
                response.Message = "Set created.";
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
