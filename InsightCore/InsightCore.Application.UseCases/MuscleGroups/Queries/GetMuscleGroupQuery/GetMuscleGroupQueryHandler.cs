using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.MuscleGroups.Queries.GetMuscleGroupQuery
{
    public class GetMuscleGroupQueryHandler : IRequestHandler<GetMuscleGroupQuery, Response<MuscleGroupDto>>
    {
        private readonly IMuscleGroupsRepository _repository;
        private readonly IMapper _mapper;

        public GetMuscleGroupQueryHandler(IMuscleGroupsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<MuscleGroupDto>> Handle(GetMuscleGroupQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<MuscleGroupDto>();
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Muscle group not found.";
                    return response;
                }
                response.Data = _mapper.Map<MuscleGroupDto>(entity);
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
