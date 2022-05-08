using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            var plans = await _tenantDbContext.ProductPlans!.Where(pp=>pp.ProductId==productId).ToListAsync();
            return _mapper.Map<IEnumerable<ProductPlanDto>>(plans);
        }

        public async Task<ProductPlanDto> GetProductPlanById(Guid productId, Guid planId)
        {
            var plan = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x => x.ProductId == productId && x.PlanId==planId);
            return _mapper.Map<ProductPlanDto>(plan);
        }

        public async Task<ProductPlanDto> AddProductPlan(ProductPlanForCreationDto planForCreationDto)
        {
            var product = await _tenantDbContext.Products!.FirstOrDefaultAsync(x => x.ProductId == planForCreationDto.ProductId);
            var plan = _mapper.Map<ProductPlan>(planForCreationDto);
            await _tenantDbContext.ProductPlans!.AddAsync(plan);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<ProductPlanDto>(plan);
        }

        public async Task DeleteProductPlan(Guid productId,Guid planId)
        {
            var plan = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x => x.PlanId == planId && x.ProductId == productId);

            if (plan != null) _tenantDbContext.ProductPlans!.Remove(plan);
        }
    }
}