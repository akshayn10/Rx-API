using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOn;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[Route("api/subscription/addOn")]
[ApiController]
public class AddOnController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AddOnController> _logger;

    public AddOnController(IMediator mediator,ILogger<AddOnController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // [HttpGet("{addOnId}",Name = "Get Add On By id")]
    // [SwaggerOperation(summary:"Get All Addon D tos")]
    // public IActionResult GetAddOnDtos(string addOnId)
    // {
    //     var addOnGuid = new Guid(addOnId);
    //     var addOnDtos = _mediator.Send(new GetAddOnByIdUseCase(addOnGuid));
    //     return Ok(addOnDtos);
    // }
    //
    //
    // public IActionResult GetAddOnDto(string productId,string addOnId)
    //
    //
    // [HttpPost("{productId}",Name = "CreateAddOn")]
    // [SwaggerOperation(summary: "Add Addon for a product")]
    // public IActionResult CreateAddOn(string productId, [FromBody] AddOnForCreationDto addOnForCreationDto)
    // {
    //     var productGuid = new Guid(productId);
    //     var createdAddOn = _mediator.Send(new AddAddOnUseCase(productGuid, addOnForCreationDto));
    //     return CreatedAtAction(nameof(), new {id = createdProduct.productId}, createdProduct)
    // }
}