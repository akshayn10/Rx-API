
using System.Diagnostics;
using System.Net.Http.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Primary;


namespace Rx.Domain.Services.Primary
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IPrimaryDbContext _primaryDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        private Organization newOrg = new Organization()
        {
            Id=new Guid("B39734B6-DAEE-40D4-A9E3-54B71D6D6672") ,
            LogoURL="dsjifbnjofrn",
            Name="Akshayan",
            TenantId= new Guid("B39734B6-DAEE-40D4-A9E3-54B71D6D6672")

        };
        

        public OrganizationService(IPrimaryDbContext primaryDbContext, ILogger<PrimaryServiceManager> logger, IMapper mapper)
        {
            _primaryDbContext = primaryDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges)
        {
            var organizations = await _primaryDbContext.Organizations!.ToListAsync();
            
            return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
        }
        public async Task<OrganizationDto> CreateOrganizationAsync(OrganizationForCreationDto organizationForCreationDto)
        {
           var organizationEntity = _mapper.Map<Organization>(organizationForCreationDto);
           Debug.Assert(_primaryDbContext.Organizations != null, "_primaryContext.Organizations != null");
           await _primaryDbContext.Organizations.AddAsync(organizationEntity);
            await _primaryDbContext.SaveChangesAsync();
            var organizationDto = _mapper.Map<OrganizationDto>(organizationEntity);

            return organizationDto;
        }

        public async Task AddPaymentGatewayIdForOrganization(Guid organizationId, string paymentGatewayId)
        {
            var organization = await _primaryDbContext.Organizations!.FindAsync(organizationId);
            organization.PaymentGatewayId = paymentGatewayId;
            await _primaryDbContext.SaveChangesAsync();
        }

        public async Task AddPaymentMethodIdForOrganization(Guid organizationId, string paymentMethodId)
        {
            var organization = await _primaryDbContext.Organizations!.FindAsync(organizationId);
            organization.PaymentMethodId = paymentMethodId;
            await _primaryDbContext.SaveChangesAsync();
        }
    }
}
