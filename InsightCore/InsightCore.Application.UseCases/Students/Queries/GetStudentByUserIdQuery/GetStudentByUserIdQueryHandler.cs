using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Coaches.Queries.GetCoachByUserIdQuery;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Students.Queries.GetStudentByUserIdQuery
{
    public class GetStudentByUserIdQueryHandler : IRequestHandler<GetStudentByUserIdQuery, Response<StudentDto>>
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly IMapper _mapper;

        public GetStudentByUserIdQueryHandler(IStudentsRepository studentsRepository, IMapper mapper)
        {
            _studentsRepository = studentsRepository;
            _mapper = mapper;
        }

        public async Task<Response<StudentDto>> Handle(GetStudentByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<StudentDto>();
                var student = await _studentsRepository.GetByUserIdAsync(request.UserId);

                if (student is null)
                {
                    response.IsSuccess = true;
                    response.Message = "Student not found.";
                    return response;
                }

                var studentDto = _mapper.Map<StudentDto>(student);
                // Retornar respuesta exitosa
                return new Response<StudentDto>
                {
                    Data = studentDto,
                    IsSuccess = true,
                    Message = "Student found."
                };
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<StudentDto> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
