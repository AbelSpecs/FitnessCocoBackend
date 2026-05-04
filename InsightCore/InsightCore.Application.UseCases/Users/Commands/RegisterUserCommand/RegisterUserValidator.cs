using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.UserName).NotNull().NotEmpty();
            RuleFor(u => u.Password).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(u => u.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(u => u.FirstName).NotNull().NotEmpty();
            RuleFor(u => u.LastName).NotNull().NotEmpty();  
            RuleFor( u => u.Birthdate).NotNull().NotEmpty();
        }
    }
}
