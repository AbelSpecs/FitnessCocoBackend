using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleNameQuery
{
    public class GetExercisesByMuscleNameQueryHandler : IRequestHandler<GetExercisesByMuscleNameQuery, Response<IEnumerable<ExerciseDto>>>
    {
        private readonly IExercisesRepository _repository;
        private readonly IMapper _mapper;

        public GetExercisesByMuscleNameQueryHandler(IExercisesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ExerciseDto>>> Handle(GetExercisesByMuscleNameQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<ExerciseDto>>();
            try
            {
                // repository does not have direct method for name; use GetAll and filter by muscle name via navigation
                var all = await _repository.GetAllAsync();
                var filtered = all.Where(e => e.MuscleGroup != null && e.MuscleGroup.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                response.Data = _mapper.Map<IEnumerable<ExerciseDto>>(filtered);
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
