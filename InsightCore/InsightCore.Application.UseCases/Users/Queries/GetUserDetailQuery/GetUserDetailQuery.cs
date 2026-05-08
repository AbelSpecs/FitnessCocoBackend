using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Queries.GetUserRolesDetailQuery
{
    public class GetUserDetailQuery : IRequest<Response<UserDetailsDto>>
    {
        public int UserId { get; set; }
    }
}
