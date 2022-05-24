using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Product;
using Rx.Domain.DTOs.Tenant.Product;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get all products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _mediator.Send(new GetProductVmsUseCase());
            return Ok(products);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get product by id")]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdUseCase(id));
            
            return Ok(product);

        }

        [HttpPost(Name = "CreateProduct")]
        [SwaggerOperation(Summary = "Create a new product")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductForCreationDto productForCreationDto)
        {
            if (productForCreationDto is null)
            {
                return BadRequest("Body is empty");
            }

            var createdProduct = await _mediator.Send(new AddProductUseCase(productForCreationDto));
            return CreatedAtAction(nameof(GetProductById), new {id = createdProduct.productId}, createdProduct);

        }


        [HttpGet("customer/{customerId:guid}")]
        [SwaggerOperation(Summary = "Get all products for a customer")]
        public async Task<IActionResult> GetProductsForCustomer(Guid customerId)
        {
            var products = await _mediator.Send(new GetProductsForCustomerUseCase(customerId));
            return Ok(products);
        }
    }
}
