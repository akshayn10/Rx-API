using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Product;
using Rx.Domain.DTOs.Tenant.Product;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _mediator.Send(new GetProductsUseCase());
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdUseCase(id));
            return Ok(product);

        }

        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto productForCreationDto)
        {
            if (productForCreationDto is null)
            {
                return BadRequest("Body is empty");
            }

            var createdProduct = await _mediator.Send(new AddProductUseCase(productForCreationDto));
            return CreatedAtAction(nameof(GetProductById), new {id = createdProduct.productId}, createdProduct);

        }

        [HttpGet("customers/{productId}")]
        public async Task<IActionResult> GetCustomersForProduct(Guid productId)
        {
            var customers = await _mediator.Send(new GetCustomersForProductUseCase(productId));
            return Ok(customers);
        }
    }
}
