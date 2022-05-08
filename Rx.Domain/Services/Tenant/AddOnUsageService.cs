using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant;

public class AddOnUsageService: IAddOnUsageService
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public AddOnUsageService(ITenantDbContext tenantDbContext,ILogger logger,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<AddOnUsageDto> CreateAddOnUsage(Guid subscriptionId, Guid addOnId, AddOnUsageForCreationDto addOnUsageForCreationDto)
    {
        var addOnUsage = _mapper.Map<AddOnUsage>(addOnUsageForCreationDto);
        var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
        var addOn = await _tenantDbContext.AddOns!.FindAsync(addOnId);
        if(subscription==null || addOn==null)
        {
            throw new Exception("Subscription or AddOn not found");
        }
        await _tenantDbContext.AddOnUsages!.AddAsync(addOnUsage);
        await _tenantDbContext.SaveChangesAsync();
        return _mapper.Map<AddOnUsageDto>(addOnUsage);
        
    }
}