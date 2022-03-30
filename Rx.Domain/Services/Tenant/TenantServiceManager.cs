
using AutoMapper;
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
        public TenantServiceManager(ITenantDbContext tenantDbContext, IMapper mapper)
        {
            _organizationCustomerService = new Lazy<IOrganizationCustomerService>(() => new OrganizationCustomerService(tenantDbContext, mapper));
            _productService = new Lazy<IProductService>(() => new ProductService(tenantDbContext, mapper));
            _subscriptionService = new Lazy<ISubscriptionService>(() => new SubscriptionService(tenantDbContext,  mapper));
            _billingService = new Lazy<IBillingService>(() => new BillingService(tenantDbContext,mapper));
        }
        public IOrganizationCustomerService OrganizationCustomerService => _organizationCustomerService.Value;
        public IProductService ProductService => _productService.Value;
        public ISubscriptionService SubscriptionService => _subscriptionService.Value;
        public IBillingService BillingService => _billingService.Value;

    }
}
