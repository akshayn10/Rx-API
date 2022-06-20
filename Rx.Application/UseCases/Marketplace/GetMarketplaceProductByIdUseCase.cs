using AutoMapper;
using MediatR;
using Rx.Domain.DTOs.Marketplace;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Marketplace;

public record GetMarketplaceProductByIdUseCase(Guid ProductId):IRequest<MarketplaceProductDto>;

public class GetMarketplaceProductByIdUseCaseHandler : IRequestHandler<GetMarketplaceProductByIdUseCase, MarketplaceProductDto>
{
    private readonly IPrimaryDbContext _primaryDbContext;
    private readonly IMapper _mapper;

    public GetMarketplaceProductByIdUseCaseHandler(IPrimaryDbContext primaryDbContext,IMapper mapper)
    {
        _primaryDbContext = primaryDbContext;
        _mapper = mapper;
    }
    public async Task<MarketplaceProductDto> Handle(GetMarketplaceProductByIdUseCase request, CancellationToken cancellationToken)
    {
        var result =  await _primaryDbContext.MarketplaceProducts.FindAsync(request.ProductId);
        return _mapper.Map<MarketplaceProductDto>(result);
    }
}