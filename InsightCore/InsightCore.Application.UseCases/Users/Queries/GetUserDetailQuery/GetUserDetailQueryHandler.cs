using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Queries.GetUserRolesDetailQuery
{
    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, Response<UserDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<UserDetailsDto>> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = new UserDetailsDto();

                var detail = await _unitOfWork.Users.GetUserDetailByUserIdAsync(request.UserId);
                if (detail.Student == null && detail.Coach == null)
                {
                    return new Response<UserDetailsDto> { IsSuccess = false, Message = "User detail not found." };
                }
                if (detail.Student != null)
                {
                    var coachStudent = await _unitOfWork.CoachStudents.GetByStudentAsync(detail.Student.Id);

                    // Validamos que la relación exista antes de proceder
                    if (coachStudent != null && coachStudent.Status)
                    {
                        detail.Coach = await _unitOfWork.Coaches.GetByIdAsync(coachStudent.CoachId);                       
                    }

                    var userStudent = await _unitOfWork.Users.GetByIdAsync(detail.Student.UserId);
                    dto.Student = _mapper.Map<StudentDetailDto>(userStudent);
                    _mapper.Map(detail.Student, dto.Student);
                }
                // Mapea cada parte por separado dentro del DTO                                        

            if (detail.Coach != null)
                {
                    dto.Coach = _mapper.Map<CoachDetailDto>(detail.Coach);
                    var userCoach = await _unitOfWork.Users.GetByIdAsync(detail.Coach.UserId);
                    dto.Coach.FirstName = userCoach.FirstName;
                    dto.Coach.LastName = userCoach.LastName;
                }

                return new Response<UserDetailsDto> { IsSuccess = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new Response<UserDetailsDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
