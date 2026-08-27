using FluentValidation;

namespace InsightCore.Application.UseCases.Users.Commands.ResetPasswordCommand
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código o token de recuperación es requerido.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("La nueva contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden.")
                .When(x => !string.IsNullOrEmpty(x.ConfirmPassword));
        }
    }
}
