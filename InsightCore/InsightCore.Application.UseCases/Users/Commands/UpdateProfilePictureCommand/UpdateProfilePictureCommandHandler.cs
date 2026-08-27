using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Users.Commands.UpdateProfilePictureCommand
{
    public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, Response<UserDto>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UpdateProfilePictureCommandHandler(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<Response<UserDto>> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<UserDto>();

            var user = await _usersRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado.";
                return response;
            }

            // Actualizar y persistir
            user.ProfilePicture = request.ProfilePicture ?? user.ProfilePicture;
            user.BannerPicture = request.BannerPicture ?? user.BannerPicture;

            var updated = await _usersRepository.UpdateAsync(user);
            if (!updated)
            {
                response.IsSuccess = false;
                response.Message = "No se pudo actualizar el usuario.";
                return response;
            }

            response.Data = _mapper.Map<UserDto>(user);
            response.IsSuccess = true;
            response.Message = "Imágenes actualizadas.";
            return response;
        }
    }
}
