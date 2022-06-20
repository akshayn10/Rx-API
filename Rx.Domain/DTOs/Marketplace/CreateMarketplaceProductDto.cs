namespace Rx.Domain.DTOs.Marketplace;

public record CreateMarketplaceProductDto (
    string ProductId,
    string? Description,
    string? Name,
    string? LogoUrl,
    int TrialDays,
    string? ProviderName,
    string? RedirectUrl
    );