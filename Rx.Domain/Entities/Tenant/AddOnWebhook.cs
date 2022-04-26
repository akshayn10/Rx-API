using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Tenant;

public class AddOnWebhook
{
    [Key] 
    [Column]
    public Guid AddOnWebhookId { get; set; }
    
    public Guid SenderAddOnWebhookId { get; set; }
     
    [ForeignKey(nameof(AddOn))]
    public Guid AddOnId { get; set; }
    public AddOn? AddOn { get; set; }
    
       
    

}