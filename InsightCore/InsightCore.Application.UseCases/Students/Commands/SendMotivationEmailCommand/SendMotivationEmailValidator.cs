using FluentValidation;

namespace InsightCore.Application.UseCases.Students.Commands.SendMotivationEmailCommand
{
    public class SendMotivationEmailValidator : AbstractValidator<SendMotivationEmailCommand>
    {
        public SendMotivationEmailValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("El mensaje motivacional no puede estar vacío.")
                .MaximumLength(1500).WithMessage("El mensaje motivacional no puede exceder los 1500 caracteres.");

            RuleFor(x => x)
                .Must(x => x.StudentId > 0 || !string.IsNullOrWhiteSpace(x.StudentEmail))
                .WithMessage("Debe proporcionar un StudentId válido o un correo electrónico.");

            RuleFor(x => x.StudentEmail)
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.StudentEmail));
        }
    }
}
