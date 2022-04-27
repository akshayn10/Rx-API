using AutoMapper;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Entities.Tenant;

namespace Rx.API.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            //Organization
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationForCreationDto,Organization>();
            //OrganizationCustomer
            CreateMap<OrganizationCustomer, OrganizationCustomerDto>();
            CreateMap<OrganizationCustomerForCreationDto,OrganizationCustomer>();
            //Product
            CreateMap<Product,ProductDto>();
            CreateMap<ProductForCreationDto,Product>();
            //Subscription
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<SubscriptionForCreationDto, Subscription>();
            //ProductPlan
            CreateMap<ProductPlan, ProductPlanDto>();
            CreateMap<ProductPlanForCreationDto, ProductPlan>();

        }
    }
}
