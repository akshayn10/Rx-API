using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Customer;

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
        [Route("customerStats")]
        public async Task<IActionResult> GetCustomersDetails()
        {
            var stats = await _mediator.Send(new GetCustomerStatsUseCase() );
            return Ok(stats);
        }
    }
}
    