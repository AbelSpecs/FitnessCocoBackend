using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Commands.UpdateCoachCommand
{
    public class UpdateCoachCommand : IRequest<Response<InsightCore.Application.DTO.CoachDto>>
    {
        public int Id { get; set; }
        public string? Bio { get; set; }
        public string? Certifications { get; set; }
        public int YearsOfExperience { get; set; } = 0;

        // Opcional: actualizar imágenes asociadas al usuario
        public string? ProfilePicture { get; set; }
        public string? BannerPicture { get; set; }
    }
}
