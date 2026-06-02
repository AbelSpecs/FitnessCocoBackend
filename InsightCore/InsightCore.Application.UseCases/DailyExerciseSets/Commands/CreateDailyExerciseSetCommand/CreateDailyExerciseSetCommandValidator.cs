using FluentValidation;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.CreateDailyExerciseSetCommand
{
    public class CreateDailyExerciseSetCommandValidator : AbstractValidator<CreateDailyExerciseSetCommand>
    {
        public CreateDailyExerciseSetCommandValidator()
        {
            RuleFor(x => x.Set).NotNull();
            RuleFor(x => x.Set.DailyStudentExerciseId).GreaterThan(0);
            RuleFor(x => x.Set.SetNumber).GreaterThan(0);
            RuleFor(x => x.Set.TargetReps).GreaterThan(0);
            RuleFor(x => x.Set.TargetWeight).GreaterThanOrEqualTo(0);
        }
    }
}
