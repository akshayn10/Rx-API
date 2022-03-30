
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Primary;


namespace Rx.Domain.Services.Primary
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IPrimaryDbContext _primaryContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrganizationService(IPrimaryDbContext primaryContext,ILogger logger, IMapper mapper)
        {
            _primaryContext = primaryContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges)
        {
            _logger.LogInformation("Hi da");
            var organizations = await _primaryContext.Organizations.ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(OrganizationForCreationDto organizationForCreationDto)
        {
           var organizationEntity = _mapper.Map<Organization>(organizationForCreationDto);
            _primaryContext.Organizations.AddAsync(organizationEntity);
            _primaryContext.SaveChangesAsync();
            var organizationDto = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationDto;
        }
    } 
}
