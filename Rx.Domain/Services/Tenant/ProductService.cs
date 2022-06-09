using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;


namespace Rx.Domain.Services.Tenant
{
    public class ProductService : IProductService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger<ITenantServiceManager> _logger;
        private readonly IMapper _mapper;
        private readonly IBlobStorage _blobStorage;

        public ProductService(ITenantDbContext tenantDbContext,ILogger<ITenantServiceManager> logger, IMapper mapper,IBlobStorage blobStorage)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
            _blobStorage = blobStorage;
        }

        public async Task<IEnumerable<ProductVm>> GetProductVms()
        {
            var products = await _tenantDbContext.Products!.
                Select(p => new
                {
                    p.ProductId,p.Name,p.LogoURL,p.RedirectURL,p.WebhookSecret
                }).ToListAsync();
            var planCount =await _tenantDbContext.ProductPlans!.GroupBy(p=>p.PlanId).Select(group=>new {group.Key,Count=group.Count()}).ToListAsync();
            var addOnCount =await _tenantDbContext.AddOns!.GroupBy(p=>p.AddOnId).Select(group=>new {group.Key,Count=group.Count()}).ToListAsync();

            var productVms = from p in products
                join pc in planCount on p.ProductId equals pc.Key
                join ac in addOnCount on p.ProductId equals ac.Key
                select (
                    new ProductVm(
                        ProductId: p.ProductId.ToString(), Name: p.Name, LogoURL: p.LogoURL, RedirectURL:p.RedirectURL,
                        PlanCount: pc.Count, AddOnCount: ac.Count,WebhookSecret:p.WebhookSecret
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
            _logger.LogInformation(productForCreationDto.ToString());
            var fileName=string.Empty;
            _logger.LogInformation("Upload Started");
            var logoImage = productForCreationDto.LogoImage;
            if (logoImage.Length > 0)
            {
                await using var fileStream = new FileStream(logoImage.FileName, FileMode.Create);
                _logger.LogInformation("file found");
                await logoImage.CopyToAsync(fileStream);
                fileName = fileStream.Name;
            }
            var stream = File.OpenRead(logoImage.FileName);
            var url =await _blobStorage.UploadFile(stream);
            _logger.LogInformation("Upload Completed");
            stream.Close();
            File.Delete(fileName);

            const string webhookSubscriptionSecretPrefix = "whs_";
            var product = new Product
            {
                Name = productForCreationDto.Name,
                Description = productForCreationDto.Description,
                WebhookURL = productForCreationDto.WebhookURL,
                WebhookSecret = webhookSubscriptionSecretPrefix + Guid.NewGuid().ToString("N"),
                LogoURL = url,
                FreeTrialDays = productForCreationDto.FreeTrialDays,
                RedirectURL = productForCreationDto.RedirectUrl

            };
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
        
        public async Task<string> DeleteProduct(Guid productId)
        {
            var product = await _tenantDbContext.Products!.FirstOrDefaultAsync(x=>x.ProductId==productId);
            if (product == null)
            {
                return "Product not found";
            }
            _tenantDbContext.Products!.Remove(product);
            await _tenantDbContext.SaveChangesAsync();
            return "Product deleted";
        }

       
        public async Task<ProductDto> UpdateProduct(Guid productId, ProductForUpdateDto productForUpdateDto)
        {
            var product = await _tenantDbContext.Products!.FindAsync(productId);
            if (product == null)
            {
                throw new NullReferenceException("Product not found");
            }
            var fileName=string.Empty;
            _logger.LogInformation("Upload Started");
            var logoImage = productForUpdateDto.LogoImage;
            if (logoImage.Length > 0)
            {
                await using var fileStream = new FileStream(logoImage.FileName, FileMode.Create);
                _logger.LogInformation("file found");
                await logoImage.CopyToAsync(fileStream);
                fileName = fileStream.Name;
            }
            var stream = File.OpenRead(logoImage.FileName);
            var url =await _blobStorage.UploadFile(stream);
            _logger.LogInformation("Upload Completed");
            stream.Close();
            File.Delete(fileName);

            var oldFileName = product.LogoURL!.Substring(56);
            await _blobStorage.DeleteFile(oldFileName);
            _logger.LogInformation("Old image deleted");

            product.Name = productForUpdateDto.Name;
            product.Description = productForUpdateDto.Description;
            product.LogoURL = url;
            product.WebhookURL = productForUpdateDto.WebhookURL;
            product.FreeTrialDays = productForUpdateDto.FreeTrialDays;
            product.RedirectURL = productForUpdateDto.RedirectUrl;
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }
    }
}

