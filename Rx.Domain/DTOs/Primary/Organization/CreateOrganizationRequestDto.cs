using Microsoft.AspNetCore.Http;
using Rx.Domain.Entities.Primary;

namespace Rx.Domain.DTOs.Primary.Organization;

public record CreateOrganizationRequestDto(string? Name,string? Description,IFormFile? LogoImage,string? Email,string? AccountOwnerId,string? AddressLine1,string? AddressLine2,string? City,string? State,string? Country);

