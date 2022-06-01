using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary
{
    public class OrganizationUser
    {
        [Key]
        [Column("OrganizationUserId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Organization name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }
    }
}