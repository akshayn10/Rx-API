namespace Rx.Domain.DTOs.Primary.Organization
{
    public record OrganizationDto(Guid Id, string Name, Guid TenantId, string Description, string LogoURL);

}
