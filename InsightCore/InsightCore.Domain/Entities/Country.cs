using InsightCore.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    [Table("Countries")]
    public class Country : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

    }
}
