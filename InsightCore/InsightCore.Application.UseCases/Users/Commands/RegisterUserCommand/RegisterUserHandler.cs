using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Configuration;


namespace InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Response<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public RegisterUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Response<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<UserDto>();
                // 1. Validar si el usuario ya existe (opcional pero recomendado)
                var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);

                if (existingUser is not null)
                {
                    response.IsSuccess = true;
                    response.Message = "El usuario ya existe.";
                    return response;
                }

                // 2. Mapear el Command (Input) a la Entidad de Dominio
                // Aquí es donde usualmente encriptarías la contraseña
                var userEntity = _mapper.Map<User>(request);
                userEntity.Created = DateTime.Now;
                userEntity.CreatedBy = "System";
                userEntity.EmailConfirmed = false;
                userEntity.Status = true;

                // 3. Generar Token EXCLUSIVO para la activación por email
                string emailToken = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32))
                    .Replace("+", "-").Replace("/", "_").Replace("=", "");

                userEntity.EmailConfirmationToken = emailToken;
                userEntity.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);

                // ---ENCRIPTACIÓN DE CONTRASEÑA ---                
                userEntity.SetSecurePassword(request.Password);
                // ----------------------------------

                // 4. Si no existe, registrar
                await _unitOfWork.Users.RegisterUser(userEntity);

                // Construir link de confirmación y enviar correo
                try
                {
                    var frontend = _configuration["AppSettings:FrontendUrl"]?.TrimEnd('/') ?? string.Empty;
                    var confirmationLink = $"{frontend}/confirm-email?userId={userEntity.Id}&token={Uri.EscapeDataString(userEntity.EmailConfirmationToken)}";

                    await _emailService.SendConfirmationEmailAsync(userEntity.Email, confirmationLink);
                }
                catch (Exception ex)
                {
                    // No interrumpir el flujo de registro por fallo en el envío de correo
                    // Loguear si existe un logger en la clase (no agregado aquí para mantener cambios mínimos)
                }

                // 5. Mapear el resultado de vuelta al DTO
                var userDto = _mapper.Map<UserDto>(userEntity);

                // 6. Retornar respuesta exitosa
                return new Response<UserDto>
                {
                    Data = userDto,
                    IsSuccess = true,
                    Message = "Usuario registrado. Por favor, revisa tu correo."
                };
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<UserDto> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        
        }
    }
}
