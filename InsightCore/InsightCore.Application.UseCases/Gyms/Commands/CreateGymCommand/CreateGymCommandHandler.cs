using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Commands.CreateGymCommand
{
    public class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, Response<GymDto>>
    {
        private readonly IGymsRepository _gymsRepository;
        private readonly IMapper _mapper;

        public CreateGymCommandHandler(IGymsRepository gymsRepository, IMapper mapper)
        {
            _gymsRepository = gymsRepository;
            _mapper = mapper;
        }

        public async Task<Response<GymDto>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<GymDto>();
            try
            {
                var entity = _mapper.Map<Gym>(request.Gym);
                // Set location from DTO (Longitude = X, Latitude = Y)
                entity.Location = new NetTopologySuite.Geometries.Point(request.Gym.Longitude, request.Gym.Latitude);
                var created = await _gymsRepository.InsertAsync(entity);
                var dto = _mapper.Map<GymDto>(created);
                if (created.Location != null)
                {
                    dto.Latitude = created.Location.Y;
                    dto.Longitude = created.Location.X;
                }
                response.Data = dto;
                response.IsSuccess = true;
                response.Message = "Gym created successfully.";
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
