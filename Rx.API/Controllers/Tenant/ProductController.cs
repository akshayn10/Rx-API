using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Middleware;
using Rx.Application.UseCases.Tenant.Product;
using Rx.Domain.DTOs.Tenant.Product;
using Swashbuckle.AspNetCore.Annotations;
using Rx.Domain.DTOs.Request;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/product")]
    [ApiController]
    [SubscriptionVerification]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> GetProducts([FromQuery] RequestParameters requestParameters)
        {
            var products = await _mediator.Send(new GetProductVmsUseCase(requestParameters.SearchKey??""));
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Get product by id")]
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

        [HttpDelete("{productId}")]
        [SwaggerOperation(Summary = "Delete a product")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            await _mediator.Send(new DeleteProductUseCase(productId));
            return NoContent();
        }

        [HttpPut("{productId}")]
        [SwaggerOperation(Summary = "Update a product")]
        public async Task<IActionResult> UpdateProduct(string productId,
            [FromForm] ProductForUpdateDto productForUpdateDto)
        {
            if (productForUpdateDto is null)
            {
                return BadRequest("Body is empty");
            }

            var updateProduct = await _mediator.Send(new EditProductUseCase(Guid.Parse(productId), productForUpdateDto));
            return Ok(updateProduct);
        }


    }
}