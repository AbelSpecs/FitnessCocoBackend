using InsightCore.Transversal.Common;
using MediatR;
using InsightCore.Application.DTO;

namespace InsightCore.Application.UseCases.Users.Commands.UpdateProfilePictureCommand
{
    public class UpdateProfilePictureCommand : IRequest<Response<UserDto>>
    {
        public int UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? BannerPicture { get; set; }
    }
}
