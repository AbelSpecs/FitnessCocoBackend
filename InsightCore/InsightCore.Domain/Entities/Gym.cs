

using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    [Table("Gyms")]
    public class Gym
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public Point Location { get; set; } = null!;

        [Column("CheckInRadius")]
        public int CheckInRadius { get; set; } = 50;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
