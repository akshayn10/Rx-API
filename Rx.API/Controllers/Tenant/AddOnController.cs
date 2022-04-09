using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/addon")]
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

    }
}
