using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Queries.GetUserQuery
{
    public class GetUserQuery : IRequest<Response<UserDto>>
    {
        public int Id { get; set; }
    }
}
