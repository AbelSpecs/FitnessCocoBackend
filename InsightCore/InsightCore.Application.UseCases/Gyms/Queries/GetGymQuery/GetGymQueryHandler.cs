using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Queries.GetGymQuery
{
    public class GetGymQueryHandler : IRequestHandler<GetGymQuery, Response<GymDto>>
    {
        private readonly IGymsRepository _gymsRepository;
        private readonly IMapper _mapper;

        public GetGymQueryHandler(IGymsRepository gymsRepository, IMapper mapper)
        {
            _gymsRepository = gymsRepository;
            _mapper = mapper;
        }

        public async Task<Response<GymDto>> Handle(GetGymQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<GymDto>();
            try
            {
                var entity = await _gymsRepository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Gym not found.";
                    return response;
                }
                var dto = _mapper.Map<GymDto>(entity);
                if (entity.Location != null)
                {
                    dto.Latitude = entity.Location.Y;
                    dto.Longitude = entity.Location.X;
                }
                response.Data = dto;
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
