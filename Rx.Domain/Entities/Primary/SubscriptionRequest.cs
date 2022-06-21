using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary;

public class SubscriptionRequest
{
    
    [Key] 
    [Column]
    public Guid SubscriptionRequestId { get; set; }
    public Guid SystemPlanId { get; set; }
    public Guid OrganizationId { get; set; }
    public bool SubscriptionType { get; set; }
    public DateTime RetrievedDateTime { get; set; }
}
