using AutoMapper;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Entities.Primary;

namespace Rx.Infrastructure.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationForCreationDto,Organization>();
        }
    }
}
