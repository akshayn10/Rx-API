using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Rx.Domain.Entities.Tenant
{
    public class Bill
    {
        [Key]
        [Column(name:"BillId")]
        public Guid BillId { get; set; }

        [Required(ErrorMessage = "GeneratedDate is required")]
        public DateTime GeneratedDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        public ICollection<PaymentTransaction>? PaymentTransactions { get; set; }


        [ForeignKey(nameof(Subscription))]
        public Guid SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }

    }
}
