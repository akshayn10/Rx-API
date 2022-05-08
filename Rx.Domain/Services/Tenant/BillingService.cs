

using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant
{
    public class BillingService : IBillingService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BillingService(ITenantDbContext tenantDbContext,ILogger logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BillDto> CreateBill(Guid subscriptonId, BillForCreationDto billForCreationDto)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptonId);
            
            if(subscription == null)
                throw new Exception("Subscription not found");
            
            var bill = _mapper.Map<Bill>(billForCreationDto);
            await _tenantDbContext.Bills.AddAsync(bill);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<BillDto>(bill);
        }
    }
}
