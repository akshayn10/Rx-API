using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant
{
    public class AddOn
    {
        [Key]
        [Column("AddOnId")]
        public Guid AddOnId { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "UnitOfMeasure is a required field.")]
        public string? UnitOfMeasure { get; set; }
        
        public ICollection<AddOnUsage>? AddOnUsages { get; set; }

        public ICollection<AddOnPricePerPlan>? AddOnPricePerPlans { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        

        

    }
}
