using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Marketplace;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Marketplace;

public record GetMarketplaceProductsUseCase(string SearchKey,bool HaveTrial):IRequest<IEnumerable<MarketplaceProductDto>>;

public class GetMarketplaceProductsUseCaseHandler : IRequestHandler<GetMarketplaceProductsUseCase, IEnumerable<MarketplaceProductDto>>
{
    private readonly IPrimaryDbContext _primaryDbContext;
    private readonly IMapper _mapper;

    public GetMarketplaceProductsUseCaseHandler(IPrimaryDbContext primaryDbContext,IMapper mapper)
    {
        _primaryDbContext = primaryDbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<MarketplaceProductDto>> Handle(GetMarketplaceProductsUseCase request, CancellationToken cancellationToken)
    {

        var result =  await _primaryDbContext.MarketplaceProducts.ToListAsync(cancellationToken: cancellationToken);
        var resultDto = result.Where(p => p.Name!.ToLower()
            .Split(' ').Any(a => a.StartsWith(request.SearchKey.ToLower()))).ToList();
        if (request.HaveTrial)
        {
            var trialProducts = resultDto.Where(p=>p.HaveTrial is true).ToList();
            return _mapper.Map<IEnumerable<MarketplaceProductDto>>(trialProducts);
        }
            
        return _mapper.Map<IEnumerable<MarketplaceProductDto>>(resultDto);
    }
}

