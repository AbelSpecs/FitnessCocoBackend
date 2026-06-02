using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.MuscleGroups.Commands.UpdateMuscleGroupCommand
{
    public class UpdateMuscleGroupCommandHandler : IRequestHandler<UpdateMuscleGroupCommand, Response<MuscleGroupDto>>
    {
        private readonly IMuscleGroupsRepository _repository;
        private readonly IMapper _mapper;

        public UpdateMuscleGroupCommandHandler(IMuscleGroupsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<MuscleGroupDto>> Handle(UpdateMuscleGroupCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<MuscleGroupDto>();
            try
            {
                var existing = await _repository.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Muscle group not found.";
                    return response;
                }

                existing.Name = request.Name;
                existing.Description = request.Description;
                existing.ImageUrl = request.ImageUrl;

                var updated = await _repository.UpdateAsync(existing);
                response.IsSuccess = updated;
                response.Data = _mapper.Map<MuscleGroupDto>(existing);
                response.Message = updated ? "Muscle group updated." : "Could not update muscle group.";
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
