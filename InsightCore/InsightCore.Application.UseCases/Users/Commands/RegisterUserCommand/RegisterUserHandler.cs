using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;


namespace InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Response<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                // ---ENCRIPTACIÓN DE CONTRASEÑA ---                
                userEntity.SetSecurePassword(request.Password);
                // ----------------------------------

                // 3. Si no existe, registrar
                await _unitOfWork.Users.RegisterUser(userEntity);

                // 4. Mapear el resultado de vuelta al DTO
                var userDto = _mapper.Map<UserDto>(userEntity);

                // 5. Retornar respuesta exitosa
                return new Response<UserDto>
                {
                    Data = userDto,
                    IsSuccess = true,
                    Message = "Usuario registrado con éxito."
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
