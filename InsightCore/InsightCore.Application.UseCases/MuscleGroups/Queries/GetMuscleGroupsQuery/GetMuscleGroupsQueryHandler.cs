using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.MuscleGroups.Queries.GetMuscleGroupsQuery
{
    public class GetMuscleGroupsQueryHandler : IRequestHandler<GetMuscleGroupsQuery, Response<IEnumerable<MuscleGroupDto>>>
    {
        private readonly IMuscleGroupsRepository _repository;
        private readonly IMapper _mapper;

        public GetMuscleGroupsQueryHandler(IMuscleGroupsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<MuscleGroupDto>>> Handle(GetMuscleGroupsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<MuscleGroupDto>>();
            try
            {
                var entities = await _repository.GetAllAsync();
                if (entities == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No muscle groups found.";
                    return response;
                }

                var dtos = _mapper.Map<IEnumerable<MuscleGroupDto>>(entities.ToList());
                response.Data = dtos;
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
