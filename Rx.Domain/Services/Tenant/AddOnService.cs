using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant
{
    public class AddOnService : IAddOnService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AddOnService(ITenantDbContext tenantDbContext, IMapper mapper, ILogger logger)
        {
            _tenantDbContext = tenantDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddOnDto> CreateAddOn(Guid productId, AddOnForCreationDto addOnForCreationDto)
        {
            var addOn = _mapper.Map<AddOn>(addOnForCreationDto);
            await _tenantDbContext.AddOns!.AddAsync(addOn);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<AddOnDto>(addOn);

        }

        public async Task<AddOnPricePerPlanDto> CreateAddOnPricePerPlan(Guid addOnId, Guid planId,
            AddOnPricePerPlanForCreationDto addOnPricePerPlanForCreationDto)
        {
            var addOnPricePerPlan = _mapper.Map<AddOnPricePerPlan>(addOnPricePerPlanForCreationDto);
            var plan = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x => x.PlanId == planId);
            var addOn = await _tenantDbContext.AddOns!.FirstOrDefaultAsync(x => x.AddOnId == addOnId);
            if (plan == null || addOn == null)
            {
                throw new Exception("Plan or AddOn not found");
            }

            await _tenantDbContext.AddOnPricePerPlans!.AddAsync(addOnPricePerPlan);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<AddOnPricePerPlanDto>(addOnPricePerPlan);
        }

        public async Task<string> DeleteAddOn(Guid addOnId)
        {
           
            var addOn = await _tenantDbContext.AddOns!.FirstOrDefaultAsync(x => x.AddOnId == addOnId);
            if (addOn == null)
            {
                throw new Exception("AddOn not found");
            }
            _tenantDbContext.AddOns!.Remove(addOn);
            await _tenantDbContext.SaveChangesAsync();
            return "AddOn deleted";
        }
        
        public async Task<string> DeleteAddOnPrice(Guid addOnPricePerPlanId) 
        {
            var addOnPrice = await _tenantDbContext.AddOnPricePerPlans!.FirstOrDefaultAsync(x => x.AddOnPricePerPlanId==addOnPricePerPlanId);
            if (addOnPrice == null)
            {
                throw new Exception("AddOnPrice not found");
            }
            _tenantDbContext.AddOnPricePerPlans!.Remove(addOnPrice);
            await _tenantDbContext.SaveChangesAsync();
            return "AddOnPrice deleted";
        }
        

        public async Task<AddOnDto> UpdateAddOn(Guid addOnId,Guid productId, AddOnForUpdateDto addOnForUpdateDto)
        {
            var addOn = await _tenantDbContext.AddOns!.FirstOrDefaultAsync(x => x.AddOnId == addOnId && x.ProductId == productId);
            if (addOn == null)
            {
                throw new NullReferenceException("AddOn not found");
            }
            
            addOn.Name = addOnForUpdateDto.Name;
            addOn.UnitOfMeasure = addOnForUpdateDto.UnitOfMeasure;
            
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<AddOnDto>(addOn);

        }
        

        public async Task<AddOnPricePerPlanDto> UpdateAddOnPrice(Guid addOnPricePerPlanId, AddOnPriceForUpdateDto addOnPriceForUpdateDto)
        {
            var addOnPrice = await _tenantDbContext.AddOnPricePerPlans!.FirstOrDefaultAsync(x => x.AddOnPricePerPlanId == addOnPricePerPlanId);
            if (addOnPrice == null)
            {
                throw new NullReferenceException("AddOnPrice not found");
            }

            addOnPrice.Price = addOnPriceForUpdateDto.Price;

            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<AddOnPricePerPlanDto>(addOnPrice);
        }
    }
}
