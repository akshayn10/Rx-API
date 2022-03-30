﻿using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Interfaces
{
    public interface ITenantServiceManager
    {
        IOrganizationCustomerService OrganizationCustomerService { get; }
        IProductService ProductService { get; }
        ISubscriptionService SubscriptionService { get; }
        IBillingService BillingService { get; }
        
    }
}
