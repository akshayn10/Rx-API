using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant
{
    public class AddOnUsage
    {
        [Key]
        public Guid AddOnUsageId { get; set; }
        public DateTime Date { get; set; }
        public int Unit { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        [ForeignKey(nameof(AddOn))]
        public Guid? AddOnId { get; set; }
        public AddOn? AddOn { get; set; }
        
        [ForeignKey(nameof(Subscription))]
        public Guid? SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
    }
}
