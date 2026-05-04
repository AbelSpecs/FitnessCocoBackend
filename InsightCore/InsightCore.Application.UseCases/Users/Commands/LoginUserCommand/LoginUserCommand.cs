using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Commands.LoginUserCommand
{
    public sealed record LoginUserCommand : IRequest<Response<LoginDto>>
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El usuario no debe exceder 30 caracteres")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
        public required string Password { get; set; }
    }
}
