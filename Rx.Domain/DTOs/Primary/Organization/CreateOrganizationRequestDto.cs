using Microsoft.AspNetCore.Http;
using Rx.Domain.Entities.Primary;

namespace Rx.Domain.DTOs.Primary.Organization;

public record CreateOrganizationRequestDto(string? Name,string? Description,IFormFile? LogoImage,string? Email,string? AccountOwnerId,OrganizationAddressForCreationDto OrganizationAddress);

