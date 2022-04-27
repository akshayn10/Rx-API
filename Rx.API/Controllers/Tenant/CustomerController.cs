using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Customer;
using Rx.Application.UseCases.Tenant.Product;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _mediator.Send(new GetCustomersUseCase() );
            return Ok(customers);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdUseCase(id));
            return Ok(customer);
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(OrganizationCustomerForCreationDto organizationCustomerForCreationDto)
        {
            if (organizationCustomerForCreationDto is null)
            {
                return BadRequest("Empty body");
            }

            var createdCustomer =await _mediator.Send(new AddCustomerUseCase(organizationCustomerForCreationDto));
            return CreatedAtAction(nameof(GetCustomerById), new {id = createdCustomer.CustomerId}, createdCustomer);
        }

        [HttpGet]
        [Route("stats")]
        public async Task<IActionResult> GetCustomersDetails()
        {
            var stats = await _mediator.Send(new GetCustomerStatsUseCase() );
            return Ok(stats);
        }
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetCustomersForProduct(Guid productId)
        {
            var customers = await _mediator.Send(new GetCustomersForProductUseCase(productId));
            return Ok(customers);
        }

        

    }
}
    