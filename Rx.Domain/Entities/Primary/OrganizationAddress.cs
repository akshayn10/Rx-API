using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary
{
    public class OrganizationAddress
    {
        [Key]
        public Guid OrganizationAddressId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        
        [ForeignKey(nameof(Organization))]
        public Guid OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
