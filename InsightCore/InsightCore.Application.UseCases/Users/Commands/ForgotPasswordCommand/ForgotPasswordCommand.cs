using InsightCore.Transversal.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.UseCases.Users.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordCommand : IRequest<Response<string>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
