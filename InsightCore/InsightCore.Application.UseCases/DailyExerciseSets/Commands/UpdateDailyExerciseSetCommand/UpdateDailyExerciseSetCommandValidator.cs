using FluentValidation;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.UpdateDailyExerciseSetCommand
{
    public class UpdateDailyExerciseSetCommandValidator : AbstractValidator<UpdateDailyExerciseSetCommand>
    {
        public UpdateDailyExerciseSetCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Set).NotNull();
            RuleFor(x => x.Set.DailyStudentExerciseId).GreaterThan(0);
            RuleFor(x => x.Set.SetNumber).GreaterThan(0);
            RuleFor(x => x.Set.TargetReps).GreaterThan(0);
            RuleFor(x => x.Set.TargetWeight).GreaterThanOrEqualTo(0);
        }
    }
}
