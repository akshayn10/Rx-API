using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary;

public class PaymentTransaction
{
    [Key]
    public Guid TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    [Column(TypeName = "decimal(18,4)")]
    public decimal TransactionAmount { get; set; }
    public string? TransactionDescription { get; set; }
    public string? TransactionStatus { get; set; }
        
    [ForeignKey(nameof(SystemSubscription))]
    public Guid SubscriptionId { get; set; }
    public SystemSubscription? SystemSubscription { get; set; }
}