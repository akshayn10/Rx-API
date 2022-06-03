using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;
public class PaymentTransaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TransactionAmount { get; set; }
        public string? TransactionDescription { get; set; }
        public string? TransactionStatus { get; set; }
        public string? TransactionPaymentReferenceId { get; set; }
        public string? TransactionPaymentGatewayResponse { get; set; }
        public string? TransactionCurrency { get; set; }

        [ForeignKey(nameof(Subscription))]
        public Guid SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
    }

