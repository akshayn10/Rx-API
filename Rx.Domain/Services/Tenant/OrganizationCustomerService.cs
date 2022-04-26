using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;


namespace Rx.Domain.Services.Tenant
{
    public class OrganizationCustomerService : IOrganizationCustomerService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrganizationCustomerService(ITenantDbContext tenantDbContext,ILogger logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;

        }

        public async Task<IEnumerable<OrganizationCustomerDto>> GetCustomers()
        {
            var customers =await _tenantDbContext.OrganizationCustomers.ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationCustomerDto>>(customers);
        }

        public async Task<CustomerStatsDto> GetCustomerStats()
        {
            
            var totalCustomer =  _tenantDbContext.OrganizationCustomers.Count();
            var totalActiveCustomer = (from c in _tenantDbContext.OrganizationCustomers
                join s in _tenantDbContext.Subscriptions on c.CustomerId equals s.OrganizationCustomerId
                where s.IsActive == true
                select c).Count();
            CustomerStatsDto customerStatsDto = new CustomerStatsDto(totalCustomer, totalActiveCustomer);
            return customerStatsDto;
            
        }
    }
}
