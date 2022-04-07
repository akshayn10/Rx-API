using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}
