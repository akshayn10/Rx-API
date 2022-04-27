using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Customer;
using Rx.Application.UseCases.Tenant.Subscription;
using Rx.Domain.DTOs.Tenant.Subscription;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/subscription")]
    [ApiController]
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
        public async Task<IActionResult> GetSubscriptions()
        {
            var result = await _mediator.Send(new GetSubscriptionsUseCase());
            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSubscriptionById(Guid id)
        {
            var result = await _mediator.Send(new GetSubscriptionByIdUseCase(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(
            [FromBody] SubscriptionForCreationDto subscriptionForCreationDto)
        {
            var createdSubscription = await _mediator.Send(new CreateSubscriptionUseCase(subscriptionForCreationDto));
            return CreatedAtAction(nameof(GetSubscriptionById), new {id = createdSubscription.SubscriptionId},
                createdSubscription);
        }
        [HttpGet("customer/{customerId:guid}")]
        public async Task<IActionResult> GetSubscriptionsForCustomer(Guid customerId)
        {
            var subscriptions = await _mediator.Send(new GetSubscriptionsForCustomerUseCase(customerId));
            return Ok(subscriptions);
        }
        [HttpGet("customer/{customerId:guid}/{subscriptionId:guid}")]
        public async Task<IActionResult> GetSubscriptionByIdForCustomer(Guid customerId,Guid subscriptionId)
        {
            var subscription = await _mediator.Send(new GetSubscriptionByIdForCustomerUseCase(customerId,subscriptionId));
            return Ok(subscription);
        }
    }
}