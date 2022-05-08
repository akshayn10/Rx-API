using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface IAddOnService
    {
        Task<AddOnDto> CreateAddOn(Guid productId, AddOnForCreationDto addOnForCreationDto);
        Task<AddOnPricePerPlanDto> CreateAddOnPricePerPlan(Guid addOnId,Guid planId, AddOnPricePerPlanForCreationDto addOnPricePerPlanForCreationDto);
        
    }
}
