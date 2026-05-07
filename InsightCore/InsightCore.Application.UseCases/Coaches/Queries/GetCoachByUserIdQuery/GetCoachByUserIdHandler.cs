using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachByUserIdQuery
{
    public class GetCoachByUserIdHandler : IRequestHandler<GetCoachByUserIdQuery, Response<CoachDto>>
    {
        private readonly ICoachesRepository _coachesRepository;
        private readonly IMapper _mapper;

        public GetCoachByUserIdHandler(ICoachesRepository coachesRepository, IMapper mapper)
        {
            _coachesRepository = coachesRepository;
            _mapper = mapper;
        }

        public async Task<Response<CoachDto>> Handle(GetCoachByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<CoachDto>();

                var coach = await _coachesRepository.GetByUserIdAsync(request.UserId);

                if (coach is null)
                {
                    response.IsSuccess = true;
                    response.Message = "El Coach no existe.";
                    return response;
                }

                var coachDto = _mapper.Map<CoachDto>(coach);

                // Retornar respuesta exitosa
                return new Response<CoachDto>
                {
                    Data = coachDto,
                    IsSuccess = true,
                    Message = "Coach existente."
                };
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<CoachDto> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
