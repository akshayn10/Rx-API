using System.ComponentModel.DataAnnotations;

namespace Rx.Domain.Entities.Tenant;

public class Transaction
{
    [Key]
    public Guid TransactionId { get; set; }
    
}