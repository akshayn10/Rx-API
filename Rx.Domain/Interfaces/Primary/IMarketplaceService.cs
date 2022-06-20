using Rx.Domain.DTOs.Marketplace;

namespace Rx.Domain.Interfaces.Primary;

public interface IMarketplaceService
{
    Task<string> CreateMarketplaceProduct(CreateMarketplaceProductDto createMarketplaceProductDto);
}