using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[Route("api/addOn/{productId}")]
[ApiController]
public class AddOnController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AddOnController> _logger;

    public AddOnController(IMediator mediator, ILogger<AddOnController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    
    [HttpGet]
    [SwaggerOperation(Summary = "Get all addOns")]
    public async Task<IActionResult> GetAddOnDtos(string productId)
    {
        var productGuid = new Guid(productId);
        var addOns = await _mediator.Send(new GetAddOnDtosUseCase(productGuid));
        return Ok(addOns);
    }

    [HttpGet("{addOnId}")]
    [SwaggerOperation(summary: "Get add by Id")]
    public async Task<IActionResult> GetAddOnById(string productId, string addOnId)
    {
        var productGuid = new Guid(productId);
        var addOnGuid = new Guid(addOnId);
        var addOnDto = await _mediator.Send(new GetAddOnByIdUseCase(productGuid, addOnGuid));
        return Ok(addOnDto);
    }


    [HttpPost]
    [SwaggerOperation(summary: "Add Addon for a product")]
    public async Task<IActionResult> CreateAddOn(string productId, [FromBody] AddOnForCreationDto addOnForCreationDto)
    {
        if (addOnForCreationDto is null)
            return BadRequest();
        var productGuid = new Guid(productId);
        var createdAddOn = await _mediator.Send(new AddAddOnUseCase(productGuid, addOnForCreationDto));
        return CreatedAtAction(nameof(GetAddOnById),
            new {productId = createdAddOn.ProductId, addOnId = createdAddOn.AddOnId}, createdAddOn);
    }

    [HttpPut("{addOnId}")]
    [SwaggerOperation(summary: "Update Addon")]

    public async Task<IActionResult> UpdateAddOn(Guid addOnId, Guid productId,[FromBody] AddOnForUpdateDto addOnForUpdateDto)
    {
        if (addOnForUpdateDto == null)
        {
            return BadRequest("Body is empty");
        }
        
        var updatedAddOn = await _mediator.Send(new EditAddOnUseCase(addOnId,productId, addOnForUpdateDto));
        return Ok(updatedAddOn);
    }
    
    [HttpDelete("{addOnId}")]
    [SwaggerOperation(summary: "Delete Addon")]
  
    public async Task<IActionResult> DeleteAddOn(Guid addOnId)
    {
        await _mediator.Send(new DeleteAddOnUseCase( addOnId));
        return NoContent();
    }

}