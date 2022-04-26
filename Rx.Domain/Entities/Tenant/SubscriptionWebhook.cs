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
        
        public string? CustomerEmail { get; set; }
         
        public string? CustomerName { get; set; }

        [ForeignKey(nameof(ProductPlan))]
        public Guid ProductPlanId { get; set; }

        public ProductPlan? ProductPlan { get; set; }  

        [Required(ErrorMessage = "Name is a required field.")]
        public string? Name { get; set; }



    }
}
