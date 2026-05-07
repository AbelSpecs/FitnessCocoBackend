using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Commands.UpdateGymCommand
{
    public class UpdateGymCommandHandler : IRequestHandler<UpdateGymCommand, Response<GymDto>>
    {
        private readonly IGymsRepository _gymsRepository;
        private readonly IMapper _mapper;

        public UpdateGymCommandHandler(IGymsRepository gymsRepository, IMapper mapper)
        {
            _gymsRepository = gymsRepository;
            _mapper = mapper;
        }

        public async Task<Response<GymDto>> Handle(UpdateGymCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<GymDto>();
            try
            {
                var existing = await _gymsRepository.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Gym not found.";
                    return response;
                }

                var entity = _mapper.Map<Gym>(request.Gym);
                entity.Id = request.Id;
                entity.Location = new NetTopologySuite.Geometries.Point(request.Gym.Longitude, request.Gym.Latitude);

                var updated = await _gymsRepository.UpdateAsync(entity);
                if (!updated)
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to update gym.";
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
                response.Message = "Gym updated successfully.";
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
