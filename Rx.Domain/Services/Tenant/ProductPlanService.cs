using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant
{
    public class ProductPlanService : IProductPlanService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductPlanService(ITenantDbContext tenantDbContext, IMapper mapper, ILogger logger)
        {
            _tenantDbContext = tenantDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductPlanDto>> GetProductPlans(Guid productId)
        {
            var plans = await _tenantDbContext.ProductPlans!.Where(pp => pp.ProductId == productId).ToListAsync();
            return _mapper.Map<IEnumerable<ProductPlanDto>>(plans);
        }

        public async Task<ProductPlanDto> GetProductPlanById(Guid productId, Guid planId)
        {
            var plan = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x =>
                x.ProductId == productId && x.PlanId == planId);
            return _mapper.Map<ProductPlanDto>(plan);
        }

        public async Task<ProductPlanDto> AddProductPlan(ProductPlanForCreationDto planForCreationDto)
        {
            var product =
                await _tenantDbContext.Products!.FirstOrDefaultAsync(x => x.ProductId == planForCreationDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var planDuration = planForCreationDto.Duration.Value;

            if (planForCreationDto.Duration.Period == "Year")
            {
                planDuration = planForCreationDto.Duration.Value * 12;
            }

            var plan = new ProductPlan()
            {
                Description = planForCreationDto.Description,
                Duration = planDuration,
                Name = planForCreationDto.Name,
                ProductId = planForCreationDto.ProductId,
                HaveTrial = planForCreationDto.HaveTrial,
                Price = planForCreationDto.Price
            };
            await _tenantDbContext.ProductPlans!.AddAsync(plan);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<ProductPlanDto>(plan);
        }

        public async Task<string> DeleteProductPlan(Guid productId, Guid planId)
        {
            var plan = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x =>
                x.PlanId == planId && x.ProductId == productId);

            if (plan == null)
            {
                throw new Exception("Plan not found");
            }

            _tenantDbContext.ProductPlans!.Remove(plan);
            await _tenantDbContext.SaveChangesAsync();
            return plan?.PlanId.ToString();
        }

        public async Task<ProductPlanDto> UpdateProductPlan(Guid productId,Guid planId,ProductPlanForUpdateDto planForUpdateDto)
        {
           
            var plan= await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x =>
                x.PlanId == planId && x.ProductId == productId);
            if(plan==null)
            {
                throw new NullReferenceException("Plan not found");
            }
            var planDuration = planForUpdateDto.Duration.Value;
            if (planForUpdateDto.Duration.Period == "Year")
            {
                planDuration = planForUpdateDto.Duration.Value * 12;
            }
            plan.Name= planForUpdateDto.Name;
            plan.Description= planForUpdateDto.Description;
            plan.Price= planForUpdateDto.Price;
            plan.Duration= planDuration;
            
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<ProductPlanDto>(plan);

        }
    }
}
