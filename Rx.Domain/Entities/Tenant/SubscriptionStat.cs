using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;

public class SubscriptionStat
{
    [Key]
    [Column("Stat Id")]
    public Guid SubscriptionStatsId { get; set; }
    [Required(ErrorMessage = "Change is a required field.")]
    public string? Change { get; set; }
    [Required(ErrorMessage = "SubscriptionId is a required field.")]
    public Guid SubscriptionId { get; set; }
    [Required(ErrorMessage = "Date is a required field.")]
    public DateTime Date { get; set; }
    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}