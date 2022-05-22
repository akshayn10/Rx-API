using AutoMapper;
using Azure.Storage.Blobs;
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
        private readonly ILogger<ITenantServiceManager> _logger;
        private readonly IMapper _mapper;

        public ProductService(ITenantDbContext tenantDbContext,ILogger<ITenantServiceManager> logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVm>> GetProductVms()
        {
            var products = await _tenantDbContext.Products!.
                Select(p => new
                {
                    p.ProductId,p.Name,p.LogoURL,p.RedirectURL
                }).ToListAsync();
            var planCount =await _tenantDbContext.ProductPlans!.GroupBy(p=>p.PlanId).Select(group=>new {group.Key,Count=group.Count()}).ToListAsync();
            var addOnCount =await _tenantDbContext.AddOns!.GroupBy(p=>p.AddOnId).Select(group=>new {group.Key,Count=group.Count()}).ToListAsync();

            var productVms = from p in products
                join pc in planCount on p.ProductId equals pc.Key
                join ac in addOnCount on p.ProductId equals ac.Key
                select (
                    new ProductVm(
                        ProductId: p.ProductId.ToString(), Name: p.Name, LogoURL: p.LogoURL, RedirectURL:p.RedirectURL,
                        PlanCount: pc.Count, AddOnCount: ac.Count
                    ));
            return productVms;
        }



        public async Task<ProductDto> GetProductById(Guid productId)
        {
            var product = await _tenantDbContext.Products!.FirstOrDefaultAsync(x => x.ProductId ==productId);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProduct(ProductForCreationDto productForCreationDto)
        {
            _logger.LogInformation("Upload Started");
            var blobStorageConnectionString ="DefaultEndpointsProtocol=https;AccountName=rxdevelopment;AccountKey=uRXyw2yHQamvLr0ymkSiMYCJZX3DdZuzLEhQBlg+u7h0vaOBybfkdei+l0R/SYi83//D9vkkPboj5zGwrIQVEQ==;EndpointSuffix=core.windows.net";
            var blobContainerName = "productlogo";
            var container  = new BlobContainerClient(blobStorageConnectionString, blobContainerName);
            var stream = File.OpenRead("C:/Users/Acer/Desktop/Project/enadoc.jpg");
            var path =stream.Name ;
            var extension = Path.GetExtension(path);
            var blob = container.GetBlobClient(productForCreationDto.Name+Guid.NewGuid().ToString("N")+extension);

            await blob.UploadAsync(stream);
            _logger.LogInformation("Upload Completed");

            const string webhookSubscriptionSecretPrefix = "whs_";
            var product = _mapper.Map<Product>(productForCreationDto);
            product.LogoURL = blob.OpenReadAsync().ToString();
            product.WebhookSecret = webhookSubscriptionSecretPrefix + Guid.NewGuid().ToString("N");
            await _tenantDbContext.Products!.AddAsync(product);
            await _tenantDbContext.SaveChangesAsync();
            
            
             return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<OrganizationCustomerDto>> GetCustomersForProduct(Guid productId)
        {
            var customers = await (from s in _tenantDbContext.Subscriptions
                join c in _tenantDbContext.OrganizationCustomers on s.OrganizationCustomerId equals c.CustomerId
                join pp in _tenantDbContext.ProductPlans on s.ProductPlanId equals pp.PlanId
                join p in _tenantDbContext.Products on pp.ProductId equals p.ProductId
                where p.ProductId == productId
                select c).ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationCustomerDto>>(customers);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsForCustomer(Guid customerId)
        {
         var products = await   (from s in _tenantDbContext.Subscriptions
                join c in _tenantDbContext.OrganizationCustomers on s.OrganizationCustomerId equals c.CustomerId
                join pp in _tenantDbContext.ProductPlans on s.ProductPlanId equals pp.PlanId
                join p in _tenantDbContext.Products on pp.ProductId equals p.ProductId
                where c.CustomerId == customerId
                select p).ToListAsync();
         return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
