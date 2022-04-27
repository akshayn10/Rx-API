using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.ProductPlan;
using Rx.Domain.DTOs.Tenant.ProductPlan;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/product/{productId:guid}/plan")]
    [ApiController]
    public class ProductPlanController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductPlanController> _logger;

        public ProductPlanController(IMediator mediator, ILogger<ProductPlanController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPlans(Guid productId)
        {
            var plans = await _mediator.Send(new GetProductPlansUseCase(productId));
            return Ok(plans);
        }
        [HttpGet("{planId:guid}")]
        public async Task<IActionResult> GetProductPlanById(Guid productId, Guid planId)
        {
            var plan = await _mediator.Send(new GetProductPlanByIdUseCase(productId, planId));
            return Ok(plan);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductPlan([FromBody] ProductPlanForCreationDto productPlanForCreationDto)
        {
            var createdPlan = await _mediator.Send(new AddProductPlanUseCase(productPlanForCreationDto));
            return CreatedAtAction(nameof(GetProductPlanById), new { productId = createdPlan.ProductId, planId = createdPlan.PlanId }, createdPlan);
        }

    }
}
