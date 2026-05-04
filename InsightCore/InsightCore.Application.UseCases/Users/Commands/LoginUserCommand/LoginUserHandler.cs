using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.UseCases;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Commands.LoginUserCommand
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Response<LoginDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;

        public LoginUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IJwtProvider jwtProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
        }

        public async Task<Response<LoginDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Buscar al usuario por su identificador (UserName o Email)
                var user = await _unitOfWork.Users.GetByUserNameAsync(request.UserName);

                // Regla de seguridad: No revelar si el usuario no existe o si la clave está mal.
                // Usamos el mismo mensaje para ambos casos.
                if (user == null)
                {
                    return new Response<LoginDto> { IsSuccess = false, Message = $"Error: Credenciales incorrectas." };
                }

                // 2. Verificar si la cuenta está bloqueada actualmente
                if (user.AccessFailedCount == 3)
                {
                    //var remainingTime = user.LockoutEnd!.Value - DateTime.UtcNow;
                    return new Response<LoginDto> { IsSuccess = false, Message = $"Cuenta bloqueada!" };
                }

                // 3. Verificar si el correo ya fue confirmado
                // Esto bloquea el acceso hasta que el usuario complete el registro.
                if (!user.EmailConfirmed)
                {
                    return new Response<LoginDto> { IsSuccess = false, Message = $"Debes confirmar tu correo electrónico para iniciar sesión." };
                }                

                // 4. Validar la contraseña (Inyectar servicio de hashing en el constructor
                if (!user.CheckPassword(request.Password))
                {
                    user.RegisterFailedLogin();
                    await _unitOfWork.Users.UpdateAsync(user);
                    return new Response<LoginDto> { IsSuccess = false, Message = $"Error: Credenciales incorrectas." };
                }

                // 5. GENERAR EL TOKEN
                // Aquí usamos el servicio de infraestructura a través de la interfaz
                var token = _jwtProvider.GenerateToken(user);

                user.Token = token;
                user.TokenExpiry  = DateTime.UtcNow.AddHours(8); // Mismo tiempo de expiración que el token

                // 6. Si llega aquí, todo está OK. Reseteamos intentos.
                user.ResetAccessFailedCount();
                await _unitOfWork.Users.UpdateAsync(user);

                // 7. Mapear a DTO y retornar
                var loginDto = new LoginDto(
                    token,
                    DateTime.UtcNow.AddHours(8),
                    user.UserName,
                     user.Email
                    );

                return new Response<LoginDto>
                {
                    Data = loginDto,
                    IsSuccess = true,
                    Message = "Bienvenido"
                };
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<LoginDto> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
