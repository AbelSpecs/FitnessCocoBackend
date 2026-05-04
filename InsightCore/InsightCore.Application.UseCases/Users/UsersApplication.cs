using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.UseCases;
using InsightCore.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Users
{
    public class UsersApplication : IUsersApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersApplication(IUnitOfWork unitOfWork, IMapper iMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = iMapper;
        }
        public async Task<Response<UserDto>> Authenticate(string username, string password)
        {
            var response = new Response<UserDto>();

            try
            {
                var user = await _unitOfWork.Users.Authenticate(username, password);
                response.Data = _mapper.Map<UserDto>(user);
                response.IsSuccess = true;
                response.Message = "Autenticación Exitosa!!!";
            }
            catch (InvalidOperationException)
            {
                response.IsSuccess = true;
                response.Message = "Usuario no existe";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<Response<UserDto>> RegisterUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
