
using Rx.Domain.DTOs.Primary.Organization;

namespace Rx.Domain.Interfaces.Primary
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges);
        Task<OrganizationDto> CreateOrganizationTest(OrganizationForCreationDto organizationForCreationDto);
        Task AddPaymentGatewayIdForOrganization(Guid organizationId, string paymentGatewayId);
        Task AddPaymentMethodIdForOrganization(Guid organizationId, string paymentMethodId);
        Task<Guid> CreateOrganizationAsync(CreateOrganizationRequestDto createOrganizationRequestDto);
        


    }
}
