using Rx.Domain.Entities.Primary;

namespace Rx.Domain.DTOs.Primary.Organization;

public record OrganizationAddressVm(string? addressLine1, string? addressLine2, string? city, string? state, string? country);
public record OrganizationVm(string OrganizationId,string? Name,string? Description,string? LogoUrl,string? Email,string? AccountOwnerId,OrganizationAddressVm? OrganizationAddress);