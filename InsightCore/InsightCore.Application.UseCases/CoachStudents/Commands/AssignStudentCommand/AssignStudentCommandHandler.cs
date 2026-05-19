using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.CoachStudents.Commands.AssignStudentCommand
{
    public class AssignStudentCommandHandler : IRequestHandler<AssignStudentCommand, Response<CoachStudentDto>>
    {
        private readonly ICoachStudentsRepository _coachStudentsRepository;
        private readonly IMapper _mapper;

        public AssignStudentCommandHandler(ICoachStudentsRepository coachStudentsRepository, IMapper mapper)
        {
            _coachStudentsRepository = coachStudentsRepository;
            _mapper = mapper;
        }

        public async Task<Response<CoachStudentDto>> Handle(AssignStudentCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CoachStudentDto>();
            try
            {
                var entity = _mapper.Map<CoachStudent>(request);
                var created = await _coachStudentsRepository.InsertAsync(entity);
                response.Data = _mapper.Map<CoachStudentDto>(created);
                response.IsSuccess = true;
                response.Message = "CoachStudent assigned.";    
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
