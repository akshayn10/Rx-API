﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Middleware;
using Rx.Application.UseCases.Tenant.AddOnUsage;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;


[ApiController]
[Route("api/AddOnUsage")]
[SubscriptionVerification]
[Authorize(Roles = "Admin")]
public class AddUsageController:ControllerBase
{
    private readonly IMediator _mediator;

    public AddUsageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("subscription/{subscriptionId}/dtos")]
    [SwaggerOperation(summary: "Get AddOnUsage Dtos for Subscription")]
    public async Task<IActionResult> GetAddOnUsageDtosForSubscription(string subscriptionId)
    {
        var addOnUsageDtos = await _mediator.Send(new GetAddOnUsageDtosForSubscription(Guid.Parse(subscriptionId)));
        return Ok(addOnUsageDtos);
        
    }
    [HttpGet("subscription/{subscriptionId}")]
    [SwaggerOperation(summary: "Get AddOnUsage Dtos for Subscription")]
    public async Task<IActionResult> GetAddOnUsageForSubscription(string subscriptionId)
    {
        var addOnUsageDtos = await _mediator.Send(new GetAddOnUsagesForSubscriptionUseCase(Guid.Parse(subscriptionId)));
        return Ok(addOnUsageDtos);
        
    }
    
    [HttpGet("{addOnId}")]
    [SwaggerOperation(summary: "Get AddOnUsage Dtos for AddOn")]
    public async Task<IActionResult> GetAddOnUsageDtosForAddOn(string addOnId)
    {
        
        var addOnUsageDtos = await _mediator.Send(new GetAddOnUsageDtosForAddOn(Guid.Parse(addOnId)));
        return Ok(addOnUsageDtos);
    }
    
    [HttpGet("{subscriptionId}/{addOnId}/{addOnUsageId}")]
    [SwaggerOperation(summary: "Get AddOnUsage Dto By Id")]
    public async Task<IActionResult> GetAddOnUsageDtoById(string subscriptionId, string addOnId,string addOnUsageId)
    {
        var addOnUsageDto = await _mediator.Send(new GetAddOnUsageDtoByIdUseCase(Guid.Parse(subscriptionId), Guid.Parse(addOnId),Guid.Parse(addOnUsageId)));
        return Ok(addOnUsageDto);
    }
    [HttpPost("{subscriptionId}/{addOnId}")]
    [SwaggerOperation(summary: "Create AddOnUsage")]
    public async Task<IActionResult> AddAddOnUsageDto(string subscriptionId, string addOnId,[FromBody] AddOnUsageForCreationDto addOnUsageForCreationDto)
    {
        var createdAddOnUsageDto =await _mediator.Send(
            new CreateAddOnUsageUseCase(Guid.Parse(subscriptionId), 
                Guid.Parse(addOnId),
                addOnUsageForCreationDto));
        return CreatedAtAction(nameof(GetAddOnUsageDtoById),new {subscriptionId, addOnId, addOnUsageId = createdAddOnUsageDto.AddOnUsageId}, createdAddOnUsageDto);

    }




}