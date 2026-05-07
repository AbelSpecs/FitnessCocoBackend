using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace InsightCore.Application.UseCases.Gyms.Queries.GetGymsQuery
{
    public class GetGymsQueryHandler : IRequestHandler<GetGymsQuery, Response<IEnumerable<GymDto>>>
    {
        private readonly IGymsRepository _gymsRepository;
        private readonly IMapper _mapper;

        public GetGymsQueryHandler(IGymsRepository gymsRepository, IMapper mapper)
        {
            _gymsRepository = gymsRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GymDto>>> Handle(GetGymsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GymDto>>();
            try
            {
                var entities = await _gymsRepository.GetAllAsync();
                if (entities == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No gyms found.";
                    return response;
                }

                var entitiesList = entities.ToList();
                var dtos = _mapper.Map<IEnumerable<GymDto>>(entitiesList).ToList();

                for (int i = 0; i < entitiesList.Count; i++)
                {
                    var entity = entitiesList[i];
                    var dto = dtos[i];
                    if (entity.Location != null)
                    {
                        dto.Latitude = entity.Location.Y;
                        dto.Longitude = entity.Location.X;
                    }
                }

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
