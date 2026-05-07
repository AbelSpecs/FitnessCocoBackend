using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand
{
    public sealed record RegisterUserCommand : IRequest<Response<UserDto>>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder 100 caracteres")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no debe exceder 100 caracteres")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El usuario no debe exceder 30 caracteres")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "El país es obligatorio")]
        public int? CountryId { get; set; }


        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de nacimiento")]
        public DateTime Birthdate { get; set; }
    }
}
