namespace Rx.Domain.DTOs.Primary.Organization;

public record OrganizationAddressForCreationDto(string? AddressLine1, string? AddressLine2, string? City, string? State, string? Country);
