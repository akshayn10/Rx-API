﻿using MediatR;
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

    [HttpPost]
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
    
}