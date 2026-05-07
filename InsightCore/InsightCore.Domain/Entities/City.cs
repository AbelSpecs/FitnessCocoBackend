using InsightCore.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    [Table("Cities")]
    public class City : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Column("CountryId")]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; } = null!;
    }
}
