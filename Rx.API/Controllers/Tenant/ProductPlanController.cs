using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/product/{productId}/plan")]
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
    }
}
