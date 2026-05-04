using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.UseCases
{
    public interface IUsersApplication
    {
        Task<Response<UserDto>> Authenticate(string username, string password);
        Task<Response<UserDto>> RegisterUser(string username, string password);
    }
}
