using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary
{
    public class Organization 
    {
        [Key]
        [Column("OrganizationId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Organization name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "TenantId is required")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "Description is a required field")]
        public string? Description { get; set; }

        public string? LogoURL { get; set; }
        public string? PaymentGatewayId { get; set; }
        public string? PaymentMethodId { get; set; }
        public string? AccountOwnerId { get; set; }
        
        public virtual OrganizationAddress? OrganizationAddress { get; set; }
        
        public ICollection<Bill>? Bills { get; set; }


    }
}
