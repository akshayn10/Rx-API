using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.Domain.Entities.Tenant
{
    public class SubscriptionWebhook
    {
        [Key]
        [Column("WebhookId")]
        public Guid WebhookId { get; set; }
        
        [Required(ErrorMessage = "SenderWebhookId is required")]
        public Guid SenderWebhookId { get; set; }


        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }        
        
        [Required(ErrorMessage = "PlanId is required")]
        public Guid PlanId { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "UnitOfMeasure is a required field.")]
        public string? UnitOfMeasure { get; set; }

    }
}
