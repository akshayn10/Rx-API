using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;

public class ChangeSubscriptionWebhook
{
    [Key]
    [Column("WebhookId")]
    public Guid WebhookId { get; set; }
    [Required(ErrorMessage = "SenderWebhookId is required")]
    public Guid SenderWebhookId { get; set; }
    [Required(ErrorMessage = "Customer Id is required")]
    public Guid CustomerId { get; set; }
    [Required(ErrorMessage = "Old subscription Id plan Id is required")]
    public Guid OldSubscriptionId { get; set; }
    [Required(ErrorMessage = "SubscriptionType is required")]
    public bool NewSubscriptionType { get; set; }
    [Required(ErrorMessage = "Plan Id is required")]
    public Guid NewPlanId { get; set; }
    public DateTime RetrievedDate { get; set; }
}