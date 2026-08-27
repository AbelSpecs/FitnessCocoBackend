using FluentValidation;

namespace InsightCore.Application.UseCases.Users.Commands.UpdateProfilePictureCommand
{
    public class UpdateProfilePictureCommandValidator : AbstractValidator<UpdateProfilePictureCommand>
    {
        public UpdateProfilePictureCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.ProfilePicture).MaximumLength(2000000).When(x => x.ProfilePicture != null);
            RuleFor(x => x.BannerPicture).MaximumLength(2000000).When(x => x.BannerPicture != null);
        }
    }
}
