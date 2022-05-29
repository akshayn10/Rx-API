using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Rx.Domain.Entities.Tenant
{
    public class AddOnPricePerPlan
    {
        [Key]
        public Guid AddOnPricePerPlanId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(AddOn))]
        public Guid? AddOnId { get; set; }
        public AddOn? AddOn { get; set; }

        [ForeignKey(nameof(ProductPlan))]
        public Guid? ProductPlanId { get; set; }
        public ProductPlan? ProductPlan { get; set; }
    }
}
