using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace InsightCore.Application.UseCases.Students.Queries.GetStudentsQuery
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, Response<IEnumerable<StudentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<StudentDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var students = await _unitOfWork.Students.GetAllAsync();
                var dto = students.Select(s => _mapper.Map<StudentDto>(s));
                return new Response<IEnumerable<StudentDto>> { IsSuccess = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<StudentDto>> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
