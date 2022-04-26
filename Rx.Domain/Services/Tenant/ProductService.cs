using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;


namespace Rx.Domain.Services.Tenant
{
    public class ProductService : IProductService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProductService(ITenantDbContext tenantDbContext,ILogger logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Guid> GetWebhookSecret(Guid productId)
        {
            // Product product = await _tenantDbContext.Products.FirstOrDefaultAsync(x => x.WebhookSecret == productId);
            // if (product == null)
            // {
            //     throw new Exception("Product not found");
            // }
            Guid webhookSecret = new Guid("86527D5F-AAE8-427A-8F76-4C4A8A90F8D1");

            return webhookSecret;
        }


    }
}
