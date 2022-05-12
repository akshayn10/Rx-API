using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant
{
    public class Product
    {
        [Key]
        [Column("ProductId")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Name is a required field.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RedirectURL { get; set; }
        [Required(ErrorMessage = "WebhookURL is a required field.")]
        public string? WebhookURL { get; set; }
        [Required(ErrorMessage = "Webhook Secret is a required field.")]
        public string? WebhookSecret { get; set; }
        public string? LogoURL { get; set; }
        public int FreeTrialDays { get; set; }
        public ICollection<ProductPlan>? ProductPlans { get; set; }
        public ICollection<AddOn>? AddOns { get; set; }


    }
}
