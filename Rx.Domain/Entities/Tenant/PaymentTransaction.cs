using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant
{
    public class PaymentTransaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TransactionAmount { get; set; }
        public string? TransactionDescription { get; set; }
        public string? TransactionPaymentStatus { get; set; }
        public string? TransactionPaymentReferenceId { get; set; }
        public string? TransactionPaymentGatewayResponse { get; set; }
        public string? TransactionPaymentGatewayTransactionId { get; set; }
        public string? TransactionPaymentGatewayTransactionAmount { get; set; }
        public string? TransactionCurrency { get; set; }

        [ForeignKey(nameof(Bill))]
        public Guid BillId { get; set; }
        public Bill? Bill { get; set; }
    }
}
