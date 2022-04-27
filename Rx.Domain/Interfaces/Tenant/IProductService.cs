using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<string> GetWebhookSecret(Guid productId);

        Task<ProductDto> GetProductById(Guid productId);

        Task<ProductDto> AddProduct(ProductForCreationDto productForCreationDto);
        
        Task<IEnumerable<OrganizationCustomerDto>> GetCustomersForProduct(Guid productId);


    }
}
