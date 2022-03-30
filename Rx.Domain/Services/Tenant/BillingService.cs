

using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant
{
    public class BillingService : IBillingService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly IMapper _mapper;

        public BillingService(ITenantDbContext tenantDbContext, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            //_logger = logger;
            _mapper = mapper;
        }
    }
}
