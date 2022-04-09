using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;

public class AddOnWebhook
{
    [Key]
    [Column]
    public int Id { get; set; }
    
}