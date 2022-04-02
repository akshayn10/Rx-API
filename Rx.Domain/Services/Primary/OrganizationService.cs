
using System.Diagnostics;
using System.Net.Http.Json;
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
        private readonly HttpClient _httpClient;

        private Organization newOrg = new Organization()
        {
            Id=new Guid("B39734B6-DAEE-40D4-A9E3-54B71D6D6672") ,
            LogoURL="dsjifbnjofrn",
            Name="werojkgnerk",
            TenantId= new Guid("B39734B6-DAEE-40D4-A9E3-54B71D6D6672")

        };

        public OrganizationService(IPrimaryDbContext primaryContext,ILogger logger, IMapper mapper, HttpClient httpClient)
        {
            _primaryContext = primaryContext;
            _logger = logger;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges)
        {
            Debug.Assert(_primaryContext.Organizations != null, "_primaryContext.Organizations != null");
            var organizations = await _primaryContext.Organizations.ToListAsync();
           var response = _httpClient.PostAsJsonAsync("https://webhook.site/f4d81af7-188e-4f0d-a893-339f4898c529",newOrg);
           Console.WriteLine(response.Status);
            return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);


        }

        public async Task<OrganizationDto> CreateOrganizationAsync(OrganizationForCreationDto organizationForCreationDto)
        {
           var organizationEntity = _mapper.Map<Organization>(organizationForCreationDto);
           Debug.Assert(_primaryContext.Organizations != null, "_primaryContext.Organizations != null");
           await _primaryContext.Organizations.AddAsync(organizationEntity);
            await _primaryContext.SaveChangesAsync();
            var organizationDto = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationDto;
        }
    } 
}
