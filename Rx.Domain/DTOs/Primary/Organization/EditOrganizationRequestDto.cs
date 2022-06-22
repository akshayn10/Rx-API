using Microsoft.AspNetCore.Http;

namespace Rx.Domain.DTOs.Primary.Organization;

public record EditOrganizationRequestDto(string? Name,string? Description,IFormFile? LogoImage,string? Email,string? AddressLine1,string? AddressLine2,string? City,string? State,string? Country);

