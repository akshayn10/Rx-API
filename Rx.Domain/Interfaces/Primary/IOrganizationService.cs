
using Rx.Domain.DTOs.Primary.Organization;

namespace Rx.Domain.Interfaces.Primary
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges);
        Task<OrganizationDto> CreateOrganizationAsync(OrganizationForCreationDto organizationForCreationDto);
    }
}
