using AutoMapper;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Entities.Tenant;

namespace Rx.API.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationForCreationDto,Organization>();

            CreateMap<OrganizationCustomer, OrganizationForCreationDto>();
            CreateMap<OrganizationCustomer, OrganizationCustomerDto>();

            CreateMap<Product,ProductDto>();
            CreateMap<ProductForCreationDto,Product>();

        }
    }
}
