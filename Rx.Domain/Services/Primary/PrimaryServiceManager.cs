using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Primary;



namespace Rx.Domain.Services.Primary
{
    public sealed class PrimaryServiceManager :IPrimaryServiceManager
    {
        private readonly Lazy<IOrganizationService> _organizationService;

        public PrimaryServiceManager(IPrimaryDbContext primaryDbContext,ILogger<PrimaryServiceManager> logger ,IMapper mapper,HttpClient httpClient)
        {
            _organizationService = new Lazy<IOrganizationService>(() => new OrganizationService(primaryDbContext, logger, mapper,httpClient));
        }

        public IOrganizationService OrganizationService => _organizationService.Value;
    }
}
