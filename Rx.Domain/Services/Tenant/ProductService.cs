using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;


namespace Rx.Domain.Services.Tenant
{
    public class ProductService : IProductService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProductService(ITenantDbContext tenantDbContext,ILogger logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _tenantDbContext.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<string> GetWebhookSecret(Guid productId)
        {
            // Product product = await _tenantDbContext.Products.FirstOrDefaultAsync(x => x.WebhookSecret == productId);
            // if (product == null)
            // {
            //     throw new Exception("Product not found");
            // }
            var webhookSecret = "86527D5F-AAE8-427A-8F76-4C4A8A90F8D1";

            return webhookSecret;
        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            var product = await _tenantDbContext.Products.FirstOrDefaultAsync(x => x.ProductId ==productId);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProduct(ProductForCreationDto productForCreationDto)
        {
            var product = _mapper.Map<Product>(productForCreationDto);
            await _tenantDbContext.Products.AddAsync(product);
            await _tenantDbContext.SaveChangesAsync();
             return _mapper.Map<ProductDto>(product);
        }
    }
}
