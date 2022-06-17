using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[Route("api/addOnPrice")]
[ApiController]
public class AddOnPricePerPlanController:ControllerBase
{
    private readonly IMediator _mediator;

    public AddOnPricePerPlanController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    //AddOn Price per Plan
    [HttpGet("addon/{addOnId}")]
    [SwaggerOperation(summary: "Get Addon Price per Plan")]
    public async Task<IActionResult> GetAddOnPricePerPlanDtos(string addOnId)
    {
        var addOnPricePerPlans = await _mediator.Send(new GetAddOnPricePerPlanUseCase(Guid.Parse(addOnId)));
        return Ok(addOnPricePerPlans);
    }

    [HttpGet("{pricePerPlanId}")]
    [SwaggerOperation(summary: "Get Addon Price per Plan by id")]
    public async Task<IActionResult> GetAddOnPriceById(string pricePerPlanId)
    {
        var addOnPricePerPlan = await _mediator.Send(new GetAddOnPricePerPlanByIdUseCase( Guid.Parse(pricePerPlanId)));
        return Ok(addOnPricePerPlan);
    }
    
    [HttpGet("addOnPlan/{productId}")]
    [SwaggerOperation(Summary = "Get all addOns")]
    public async Task<IActionResult> GetAddOnPerProduct(string productId)
    {
        var addOnPerProduct = await _mediator.Send(new GetAddOnPerProductUseCase(Guid.Parse(productId)));
        return Ok(addOnPerProduct);
    }
    
    [HttpPost("{addOnId}/{planId}")]
    [SwaggerOperation(summary: "Add Addon Price per Plan")]
    public async Task<IActionResult> AddAddOnPrice(string planId, string addOnId,
        [FromBody] AddOnPricePerPlanForCreationDto addOnPricePerPlanForCreationDto)
    {
        var createdAddOnPricePerPlan = await _mediator.Send(new AddAddOnPricePerPlanUseCase(Guid.Parse(addOnId), Guid.Parse(planId),addOnPricePerPlanForCreationDto));
        return CreatedAtAction(nameof(GetAddOnPriceById),
            new
            {
                pricePerPlanId = createdAddOnPricePerPlan.AddOnPricePerPlanId
            },
            createdAddOnPricePerPlan);
    }
    
  [HttpDelete("{addOnId}")]
    [SwaggerOperation(summary: "Delete Addon")]
  
  public async Task<IActionResult> DeleteAddOnPrice(Guid addOnId)
        {
            await _mediator.Send(new DeleteAddOnPricePerPlanUseCase( addOnId));
            return NoContent();
        }
  
    [HttpPut("{addOnId}")]
    [SwaggerOperation(summary: "Update AddonPrice")]
    
    public async Task<IActionResult> UpdateAddOnPrice(Guid addOnId, [FromBody] AddOnPriceForUpdateDto addOnPriceForUpdateDto)
    {
        if(addOnPriceForUpdateDto == null)
        {
            return BadRequest("Body is empty");
        }
        
        var updatedAddOnPrice = await _mediator.Send(new EditAddOnPriceUseCase(addOnId,addOnPriceForUpdateDto ));
        return Ok(updatedAddOnPrice);
        
    }
    

}