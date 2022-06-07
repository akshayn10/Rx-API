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
        [Required(ErrorMessage = "Customer Email is required")]
        public string? CustomerEmail { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Product plan Id is required")]
        public Guid ProductPlanId { get; set; }
        [Required(ErrorMessage = "SubscriptionType is required")]
        public bool SubscriptionType { get; set; }
        public DateTime RetrievedDate { get; set; }
    }
}
