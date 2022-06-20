using MediatR;
using Rx.Domain.DTOs.Marketplace;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Marketplace;

public record CreateMarketplaceProductUseCase(CreateMarketplaceProductDto CreateMarketplaceProductDto):IRequest<string>;

public class CreateMarketplaceProductUseCaseHandler : IRequestHandler<CreateMarketplaceProductUseCase, string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public CreateMarketplaceProductUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<string> Handle(CreateMarketplaceProductUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.MarketplaceService.CreateMarketplaceProduct(request.CreateMarketplaceProductDto);
    }
}