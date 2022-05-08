using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[Route("api/product/{productId}/addOn")]
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
    public async Task<IActionResult> GetAddOnDtos(string productId)
    {
        var productGuid = new Guid(productId);
        var addOns = await _mediator.Send(new GetAddOnDtosUseCase(productGuid));
        return Ok(addOns);

    }

    [HttpGet("{addOnId}")]
    [SwaggerOperation(summary: "Get All Addon D tos")]
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


    //AddOn Price per Plan
    [HttpGet("{addOnId}/price")]
    [SwaggerOperation(summary: "Get Addon Price per Plan")]
    public async Task<IActionResult> GetAddOnPricePerPlanDtos(string productId, string addOnId)
    {
        var productGuid = new Guid(productId);
        var addOnGuid = new Guid(addOnId);
        var addOnPricePerPlans = await _mediator.Send(new GetAddOnPricePerPlanUseCase(productGuid, addOnGuid));
        return Ok(addOnPricePerPlans);
    }

    [HttpGet("{addOnId}/price/{pricePerPlanId}")]
    [SwaggerOperation(summary: "Get Addon Price per Plan by id")]
    public async Task<IActionResult> GetAddOnPriceById(string productId, string addOnId, string pricePerPlanId)
    {
        var productGuid = new Guid(productId);
        var addOnGuid = new Guid(addOnId);
        var pricePerPlanGuid = new Guid(pricePerPlanId);
        var addOnPricePerPlan =
            await _mediator.Send(new GetAddOnPricePerPlanByIdUseCase(productGuid, addOnGuid, pricePerPlanGuid));
        return Ok(addOnPricePerPlan);
    }

    [HttpPost("{addOnId}/{planId}/price")]
    [SwaggerOperation(summary: "Add Addon Price per Plan")]
    public async Task<IActionResult> AddAddOnPrice(string planId, string addOnId,
        [FromBody] AddOnPricePerPlanForCreationDto addOnPricePerPlanForCreationDto)
    {
        var planGuid = new Guid(planId);
        var addOnGuid = new Guid(addOnId);
        var createdAddOnPricePerPlan =
            await _mediator.Send(new AddAddOnPricePerPlanUseCase(addOnGuid, planGuid,
                addOnPricePerPlanForCreationDto));
        return CreatedAtAction(nameof(GetAddOnPriceById),
            new
            {
                productId = planGuid,
                addOnId = createdAddOnPricePerPlan.AddOnId,
                pricePerPlanId = createdAddOnPricePerPlan.AddOnPricePerPlanId
            },
            createdAddOnPricePerPlan);
    }
}