namespace Rx.Domain.DTOs.Marketplace;

public record MarketplaceProductDto(
    Guid ProductId,
    string? Description,
    string? Name,
    string? LogoUrl,
    bool? HaveTrial,
    string? ProviderName,
    string? RedirectUrl
);
