using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface IProductService
    {
        Task<IEnumerable<ProductVm>> GetProductVms();
        Task<ProductDto> GetProductById(Guid productId);

        Task<ProductDto> AddProduct(ProductForCreationDto productForCreationDto);
        
        Task<IEnumerable<OrganizationCustomerDto>> GetCustomersForProduct(Guid productId);
        
        Task<IEnumerable<ProductDto>> GetProductsForCustomer(Guid customerId);
        


    }
}
