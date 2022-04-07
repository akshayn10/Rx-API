using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/product")]
    public class ProductController :ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator,ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

    }
}
