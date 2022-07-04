﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Middleware;
using Rx.Application.UseCases.Tenant.Customer;
using Rx.Application.UseCases.Tenant.Subscription;
using Rx.Domain.DTOs.Tenant.Subscription;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/subscription")]
    [ApiController]
    [SubscriptionVerification]
    [Authorize(Roles = "FinanceUser")]
    public class SubscriptionController:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(IMediator mediator,ILogger<SubscriptionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all subscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var result = await _mediator.Send(new GetSubscriptionsUseCase());
            return Ok(result);
        }
        [HttpGet("/deactivate/{subscriptionId}")]
        [SwaggerOperation(Summary = "Cancel subscription")]
        public async Task<IActionResult> DeactivateSubscription(string subscriptionId)
        {
           var result = await _mediator.Send(new DeactivateSubscriptionUseCase(Guid.Parse(subscriptionId)));
           return Ok(result);
        }
        
        [HttpGet("dtos")]
        [SwaggerOperation(Summary = "Get all subscriptions Dto")]
        public async Task<IActionResult> GetSubscriptionsDto()
        {
            var result = await _mediator.Send(new GetSubscriptionsDtoUseCase());
            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Get subscription by id")]
        public async Task<IActionResult> GetSubscriptionById(Guid id)
        {
            var result = await _mediator.Send(new GetSubscriptionByIdUseCase(id));
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new subscription")]
        public async Task<IActionResult> CreateSubscription(
            [FromBody] SubscriptionForCreationDto subscriptionForCreationDto)
        {
            var createdSubscription = await _mediator.Send(new CreateSubscriptionUseCase(subscriptionForCreationDto));
            return CreatedAtAction(nameof(GetSubscriptionById), new {id = createdSubscription.SubscriptionId},
                createdSubscription);
        }
        [HttpGet("customer/{customerId}")]
        [SwaggerOperation(Summary = "Get all subscriptions for a customer")]
        public async Task<IActionResult> GetSubscriptionsForCustomer(string customerId)
        {
            var customerGuid = new Guid(customerId);
            var subscriptions = await _mediator.Send(new GetSubscriptionsForCustomerUseCase(customerGuid));
            return Ok(subscriptions);
        }
        
        [HttpGet("customer/{customerId}/dto")]
        [SwaggerOperation(Summary = "Get all subscriptions Dto for a customer")]
        public async Task<IActionResult> GetSubscriptionsForCustomerDto(string customerId)
        {
            var customerGuid = new Guid(customerId);
            var subscriptions = await _mediator.Send(new GetSubscriptionsForCustomerDtoUseCase(customerGuid));
            return Ok(subscriptions);
        }
        [HttpGet("customer/{customerId:guid}/{subscriptionId:guid}")]
        [SwaggerOperation(Summary = "Get subscription for a customer")]
        public async Task<IActionResult> GetSubscriptionByIdForCustomer(Guid customerId,Guid subscriptionId)
        {
            var subscription = await _mediator.Send(new GetSubscriptionByIdForCustomerUseCase(customerId));
            return Ok(subscription);
        }
        [HttpGet("sub-stats")]
        [SwaggerOperation(Summary = "Get subscription stats")]
        public async Task<IActionResult> GetSubscriptionStats()
        {
            var stats = await _mediator.Send(new GetSubscriptionStatsUseCase());
            return Ok(stats);
        }
    }
}