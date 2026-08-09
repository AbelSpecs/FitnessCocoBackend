using InsightCore.Transversal.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.UseCases.Users.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommand : IRequest<Response<string>>
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
