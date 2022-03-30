 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

 namespace Rx.Domain.Entities.Tenant
{
    public class Subscription
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
        [Required(ErrorMessage = "IsCancelled is required")]
        public bool IsCancelled { get; set; }
        [Required(ErrorMessage = "IsTrial is required")]
        public bool IsTrial { get; set; }
        [Required(ErrorMessage = "Created Date is required")]
        public DateTime CreatedDate { get; set; }

        public ICollection<Bill> Bills { get; set; }


        [ForeignKey(nameof(OrganizationCustomer))]
        public Guid OrganizationCustomerId { get; set; }
        public OrganizationCustomer? OrganizationCustomer { get; set; }

        [ForeignKey(nameof(ProductPlan))]
        public Guid ProductPlanId { get; set; }
        public ProductPlan? ProductPlan { get; set; }





    }
}
