using FluentValidation;
using InsightCore.Application.DTO;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.AssignDailyExerciseCommand
{
    public class AssignDailyExerciseCommandValidator : AbstractValidator<AssignDailyExerciseCommand>
    {
        public AssignDailyExerciseCommandValidator()
        {
            RuleFor(x => x.Assign).NotNull();
            RuleFor(x => x.Assign.CoachId).GreaterThan(0);
            RuleFor(x => x.Assign.StudentId).GreaterThan(0);
            RuleFor(x => x.Assign.ExerciseId).GreaterThan(0);
            RuleFor(x => x.Assign.ScheduledDate).NotEmpty();

            When(x => x.Assign.DailyExerciseSets != null, () =>
            {
                RuleForEach(x => x.Assign.DailyExerciseSets).SetValidator(new DailyExerciseSetDtoValidator());
            });
        }
    }

    public class DailyExerciseSetDtoValidator : AbstractValidator<DailyExerciseSetDto>
    {
        public DailyExerciseSetDtoValidator()
        {
            RuleFor(x => x.SetNumber).GreaterThan(0);
            RuleFor(x => x.TargetReps).GreaterThan(0);
            RuleFor(x => x.TargetWeight).GreaterThanOrEqualTo(0);
        }
    }
}
