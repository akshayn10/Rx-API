using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;

public class AddOnWebhook
{
    [Key] 
    [Column]
    public Guid AddOnWebhookId { get; set; }
    
    public Guid SenderAddOnWebhookId { get; set; }
    public string? CustomerEmail { get; set; }

    public Guid AddOnId { get; set; }
    public Guid ProductPlanId { get; set; }
    public int Unit { get; set; }

}