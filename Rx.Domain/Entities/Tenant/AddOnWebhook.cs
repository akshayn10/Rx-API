using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;

public class AddOnWebhook
{
    [Key] 
    [Column]
    public Guid AddOnWebhookId { get; set; }
    public Guid SenderAddOnWebhookId { get; set; }
    public Guid AddOnId { get; set; }
    public Guid SubscriptionId { get; set; }
    public int Unit { get; set; }
    public Guid OrganizationCustomerId { get; set; }
    public DateTime RetrievedDateTime { get; set; }
}
