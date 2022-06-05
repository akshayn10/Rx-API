using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.DTOs.Tenant.ProductPlan;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface IProductPlanService
    {
        Task<IEnumerable<ProductPlanDto>> GetProductPlans(Guid productId);

        Task<ProductPlanDto> GetProductPlanById(Guid productId, Guid planId);

        Task<ProductPlanDto> AddProductPlan(ProductPlanForCreationDto planForCreationDto);

        Task<string> DeleteProductPlan(Guid productId,Guid planId);
        
        Task<ProductPlanDto> UpdateProductPlan(Guid productId, Guid planId, ProductPlanForUpdateDto planForUpdateDto);
        
    }
}
