using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Marketplace;
using Rx.Domain.DTOs.Marketplace;
using Rx.Domain.DTOs.Request;

namespace Rx.API.Controllers.MarketplaceController;

[ApiController]
[Route("api/marketplace")]
public class MarketplaceController:ControllerBase
{
    private readonly IMediator _mediator;

    public MarketplaceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] RequestParameters requestParameters)
    {
        var result = await _mediator.Send(new GetMarketplaceProductsUseCase(requestParameters.SearchKey??"",requestParameters.HaveTrial??false));
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var result = await _mediator.Send(new GetMarketplaceProductByIdUseCase(Guid.Parse(id)));
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateMarketplaceProductDto request)
    {
        var result = await _mediator.Send(new CreateMarketplaceProductUseCase(request));
        return Ok(result);
    }
}