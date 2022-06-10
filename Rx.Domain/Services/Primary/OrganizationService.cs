
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
        private readonly IPrimaryDbContext _primaryContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient? _httpClient;
        private static RetryPolicy? _retryPolicy;

        private Organization newOrg = new Organization()
        {
            Id=new Guid("B39734B6-DAEE-40D4-A9E3-54B71D6D6672") ,
            LogoURL="dsjifbnjofrn",
            Name="Akshayan",
            TenantId= new Guid("B39734B6-DAEE-40D4-A9E3-54B71D6D6672")

        };

        public OrganizationService(IPrimaryDbContext primaryContext,ILogger logger, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _primaryContext = primaryContext;
            _logger = logger;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy
                .Handle<Exception>()
                .Retry(2);
        }

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges)
        {
            Debug.Assert(_primaryContext.Organizations != null, "_primaryContext.Organizations != null");
            var organizations = await _primaryContext.Organizations.ToListAsync();
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "webHookSecret");

            var response = await _retryPolicy!.Execute(()=> _httpClient.PostAsJsonAsync("https://baeb0b32f6296cd6566129eed5eb1a12.m.pipedream.net", newOrg));

            _logger.LogInformation(response.ToString());
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
