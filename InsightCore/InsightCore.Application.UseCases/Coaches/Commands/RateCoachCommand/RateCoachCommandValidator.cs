using FluentValidation;

namespace InsightCore.Application.UseCases.Coaches.Commands.RateCoachCommand
{
    public class RateCoachCommandValidator : AbstractValidator<RateCoachCommand>
    {
        public RateCoachCommandValidator()
        {
            RuleFor(x => x.CoachId).GreaterThan(0);
            RuleFor(x => x.StudentId).GreaterThan(0);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("La valoración debe estar entre 1 y 5.");
            RuleFor(x => x.Comment).MaximumLength(1000);
        }
    }
}
