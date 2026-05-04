using FluentValidation;

namespace InsightCore.Application.UseCases.Students.Commands.DeleteStudentCommand
{
    public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
