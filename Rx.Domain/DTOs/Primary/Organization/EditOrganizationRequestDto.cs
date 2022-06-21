using Microsoft.AspNetCore.Http;

namespace Rx.Domain.DTOs.Primary.Organization;

public record EditOrganizationRequestDto(string? Name,string? Description,IFormFile? LogoImage,string? Email,OrganizationAddressForCreationDto OrganizationAddress);