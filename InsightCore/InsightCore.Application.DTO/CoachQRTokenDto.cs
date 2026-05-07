using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class CoachQRTokenDto
    {
        public int CoachId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateQRTokenDto
    {
        [Required(ErrorMessage = "El ID del coach es obligatorio")]
        public int CoachId { get; set; }

        [Required]
        [Range(1, 1440, ErrorMessage = "La validez debe ser entre 1 minuto y 24 horas")]
        public int MinutesValidity { get; set; }
    }

    public class QRTokenDto
    {
        public int CoachId { get; set; }
        public string Base64 { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class QRTokenRegistroDto
    {
        public int CoachId { get; set; }
        public string Url { get; set; }
    }
}
