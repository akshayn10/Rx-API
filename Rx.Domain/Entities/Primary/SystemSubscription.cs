using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary
{
    public class SystemSubscription
    {
        [Key]
        [Column("SubscriptionId")]
        public Guid SubscriptionId { get; set; }
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "SubscriptionType is required")]
        public bool SubscriptionType { get; set; }
        [Required(ErrorMessage = "IsCancelled is required")]
        public bool IsCancelled { get; set; }
        public string? JobId { get; set; }
        [Required(ErrorMessage = "Created Date is required")]
        public DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(SystemSubscriptionPlan))]
        public Guid ProductPlanId { get; set; }
        public SystemSubscriptionPlan? SystemSubscriptionPlan { get; set; }
        public ICollection<PaymentTransaction>? PaymentTransactions { get; set; }
    }
}
