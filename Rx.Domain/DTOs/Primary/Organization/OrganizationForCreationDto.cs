namespace Rx.Domain.DTOs.Primary.Organization
{
    public record OrganizationForCreationDto(string Name, Guid TenantId, string Description, string LogoURL);
}
