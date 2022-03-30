using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;


namespace Rx.Domain.Services.Tenant
{
    public class ProductService : IProductService
    {
        private readonly ITenantDbContext _tenantDbContext;
      //  private readonly ILogger<ITenantServiceManager> _logger;
        private readonly IMapper _mapper;

        public ProductService(ITenantDbContext tenantDbContext, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
           // _logger = logger;
            _mapper = mapper;
        }
    }
}
