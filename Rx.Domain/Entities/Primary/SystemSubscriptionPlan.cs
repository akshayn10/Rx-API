using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary
{
    public class SystemSubscriptionPlan
    {
        [Key]
        [Column("PlanId")]
        public Guid PlanId { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Price is a required field.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is a required field.")]
        public int? Duration { get; set; }

        public ICollection<SystemSubscription>? SystemSubscriptions { get; set; }
    }
}
