using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Subscription;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface IOrganizationCustomerService
    {
        Task<CustomerStatsDto> GetCustomerStats();
        Task<IEnumerable<OrganizationCustomerDto>> GetCustomers();

        Task<OrganizationCustomerDto> GetCustomerById(Guid id);

        Task<OrganizationCustomerDto> AddCustomer(OrganizationCustomerForCreationDto organizationCustomerForCreationDto);

        Task<Guid> AddPaymentMethod(string requestCustomerId, string requestLast4);
        Task<string> CreateCustomerFromWebhook(SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto);
    }
}
