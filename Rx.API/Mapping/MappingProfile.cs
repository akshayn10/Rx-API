using AutoMapper;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Entities.Tenant;

namespace Rx.Infrastructure.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationForCreationDto,Organization>();

            CreateMap<OrganizationCustomer, OrganizationForCreationDto>();
            CreateMap<OrganizationCustomer, OrganizationCustomerDto>();

        }
    }
}
