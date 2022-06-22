
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Entities.Primary;

namespace Rx.Domain.Interfaces.Primary
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges);
        Task<OrganizationDto> CreateOrganizationTest(OrganizationForCreationDto organizationForCreationDto);
        Task AddPaymentGatewayIdForOrganization(Guid organizationId, string paymentGatewayId);
        Task<Guid> AddPaymentMethodIdForOrganization(Guid organizationId, string paymentMethodId);
        Task<Guid> CreateOrganizationAsync(CreateOrganizationRequestDto createOrganizationRequestDto);
        Task<OrganizationDto> EditOrganization(Guid organizationId, EditOrganizationRequestDto editOrganizationRequestDto);
        Task<string> CreateOrganizationInStripeUseCase(SubscriptionRequest subscriptionRequest);
    }
}
