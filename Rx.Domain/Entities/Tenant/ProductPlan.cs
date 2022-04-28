using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant
{
    public  class ProductPlan
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

        public ICollection<Subscription>? Subscriptions { get; set; }
        public ICollection<AddOnPricePerPlan>? AddOnPricePerPlans { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        


    }
}
