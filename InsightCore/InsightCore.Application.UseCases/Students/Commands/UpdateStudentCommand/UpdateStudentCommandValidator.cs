using FluentValidation;

namespace InsightCore.Application.UseCases.Students.Commands.UpdateStudentCommand
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Student).NotNull();
            RuleFor(x => x.Student.UserId).GreaterThan(0);
            RuleFor(x => x.Student.Weight).GreaterThanOrEqualTo(0).When(x => x.Student.Weight.HasValue);
            RuleFor(x => x.Student.Height).GreaterThanOrEqualTo(0).When(x => x.Student.Height.HasValue);
        }
    }
}
