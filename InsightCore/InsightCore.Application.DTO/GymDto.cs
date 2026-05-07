using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class GymDto
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CheckInRadius { get; set; } = 50; // Valor por defecto si no se envía
    }
}
