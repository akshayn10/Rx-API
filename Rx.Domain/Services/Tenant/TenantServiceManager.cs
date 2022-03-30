
using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;
using Rx.Domain.Services.Tenant;

namespace Service
{
    public sealed class TenantServiceManager : ITenantServiceManager
    {
        private readonly Lazy<IOrganizationCustomerService> _organizationCustomerService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<ISubscriptionService> _subscriptionService;
        private readonly Lazy<IBillingService> _billingService;
        public TenantServiceManager(ITenantDbContext tenantDbContext,ILogger<TenantServiceManager> logger, IMapper mapper)
        {
            _organizationCustomerService = new Lazy<IOrganizationCustomerService>(() => new OrganizationCustomerService(tenantDbContext, logger, mapper));
            _productService = new Lazy<IProductService>(() => new ProductService(tenantDbContext, logger, mapper));
            _subscriptionService = new Lazy<ISubscriptionService>(() => new SubscriptionService(tenantDbContext, logger, mapper));
            _billingService = new Lazy<IBillingService>(() => new BillingService(tenantDbContext, logger,mapper));
        }
        public IOrganizationCustomerService OrganizationCustomerService => _organizationCustomerService.Value;
        public IProductService ProductService => _productService.Value;
        public ISubscriptionService SubscriptionService => _subscriptionService.Value;
        public IBillingService BillingService => _billingService.Value;

    }
}
