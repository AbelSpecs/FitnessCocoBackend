using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetStudentsListByCoachIdQuery
{
    public class GetStudentsListByCoachIdQueryHandler : IRequestHandler<GetStudentsListByCoachIdQuery, Response<StudentListByCoachDto>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IStudentsRepository _studentsRepository;   
        private readonly ICoachStudentsRepository _coachStudentsRepository; 
        private readonly IMapper _mapper;

        public GetStudentsListByCoachIdQueryHandler(
                IUsersRepository usersRepository, IStudentsRepository studentsRepository, ICoachStudentsRepository coachStudentsRepository,
                IMapper mapper)
        {
            _usersRepository = usersRepository;
            _studentsRepository = studentsRepository;
            _coachStudentsRepository = coachStudentsRepository;
            _mapper = mapper;
        }

        public async Task<Response<StudentListByCoachDto>> Handle(GetStudentsListByCoachIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<StudentListByCoachDto>();

                var listCoachStudents = await _coachStudentsRepository.GetByCoachAsync(request.CoachId);

                var listStudents = new StudentListByCoachDto();

                foreach(var coachStudent in listCoachStudents)
                {
                    var student = await _studentsRepository.GetByIdAsync(coachStudent.StudentId);
                    if (student != null)
                    {
                        var studentDto = new CoachStudentItemDto
                        {
                            StudentId = student.Id,
                            FitnessGoal = student.FitnessGoal
                        };

                        studentDto.Name = (await _usersRepository.GetByIdAsync(student.UserId))?.FirstName;
                        listStudents.Students.Add(studentDto);
                    }                    
                }               
                // Retornar respuesta exitosa
                return new Response<StudentListByCoachDto>
                {
                    Data = listStudents,
                    IsSuccess = true,
                    Message = "Students List found."
                };
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<StudentListByCoachDto> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}