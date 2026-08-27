using FluentValidation;

namespace InsightCore.Application.UseCases.Coaches.Commands.UpdateCoachCommand
{
    public class UpdateCoachCommandValidator : AbstractValidator<UpdateCoachCommand>
    {
        public UpdateCoachCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).WithMessage("Los años de experiencia no pueden ser negativos.");
            RuleFor(x => x.Bio).MaximumLength(2000);
            RuleFor(x => x.Certifications).MaximumLength(1000);
            RuleFor(x => x.ProfilePicture).MaximumLength(2000000).When(x => x.ProfilePicture != null);
            RuleFor(x => x.BannerPicture).MaximumLength(2000000).When(x => x.BannerPicture != null);
        }
    }
}
