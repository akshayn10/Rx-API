using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant
{
    public class AddOnService: IAddOnService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AddOnService(ITenantDbContext tenantDbContext,IMapper mapper,ILogger logger)
        {
            _tenantDbContext = tenantDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddOnDto> CreateAddOn(Guid productId,AddOnForCreationDto addOnForCreationDto)
        {
            var addOn = _mapper.Map<AddOn>(addOnForCreationDto);
            await _tenantDbContext.AddOns!.AddAsync(addOn);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<AddOnDto>(addOn); 
            
        }
    }
}
