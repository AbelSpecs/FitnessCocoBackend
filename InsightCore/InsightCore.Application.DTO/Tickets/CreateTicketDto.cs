using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.DTO.Tickets
{
    public class CreateTicketDto
    {
        [Required(ErrorMessage = "El nombre del bug es obligatorio")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre del bug debe tener entre 3 y 150 caracteres")]
        public string BugName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del bug es obligatoria")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        public string Description { get; set; } = string.Empty;

        public string Platform { get; set; } = "android"; // 'android' | 'ios'
        public string? AppVersion { get; set; }
        public string? DeviceModel { get; set; }
    }
}

