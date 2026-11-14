using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.DTO.Tickets
{
    public class UpdateTicketStatusDto
    {
        public bool IsAddressed { get; set; }

        [MaxLength(1000)]
        public string? ResolutionNotes { get; set; }
    }
}

