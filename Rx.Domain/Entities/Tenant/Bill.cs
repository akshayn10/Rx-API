using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Rx.Domain.Entities.Tenant;

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

        
        [ForeignKey(nameof(OrganizationCustomer))]
        public Guid CustomerId { get; set; }
        public OrganizationCustomer? OrganizationCustomer { get; set; }

    }

