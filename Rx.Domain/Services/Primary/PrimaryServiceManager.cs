using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Primary;



namespace Rx.Domain.Services.Primary
{
    public sealed class PrimaryServiceManager :IPrimaryServiceManager
    {
        private readonly Lazy<IOrganizationService> _organizationService;
        private readonly Lazy<ISystemSubscriptionService> _systemSubscriptionService;
        private readonly Lazy<ISystemSubscriptionPlanService> _systemSubscriptionPlanService;
        private readonly Lazy<ITransactionService> _transactionService;
        private readonly Lazy<IBillService> _billService;
        private readonly Lazy<IMarketplaceService> _marketplaceService;

        public PrimaryServiceManager(IPrimaryDbContext primaryDbContext,ILogger<PrimaryServiceManager> logger ,ITenantDbContext tenantDbContext,
            IMapper mapper,IEmailService emailService,IBlobStorage blobStorage,IPaymentService paymentService,IBackgroundJobClient backgroundJobClient,IUserService userService)
        {
            _organizationService = new Lazy<IOrganizationService>(() => new OrganizationService(primaryDbContext, logger, mapper,blobStorage,emailService,userService,paymentService));
            _systemSubscriptionService = new Lazy<ISystemSubscriptionService>(() => new SystemSubscriptionService(primaryDbContext,
                logger, mapper,emailService,paymentService,backgroundJobClient));
            _systemSubscriptionPlanService = new Lazy<ISystemSubscriptionPlanService>(() => new SystemSubscriptionPlanService(primaryDbContext, logger, mapper));
            _transactionService = new Lazy<ITransactionService>(() => new TransactionService(primaryDbContext, logger, mapper));
            _billService = new Lazy<IBillService>(() => new BillService(primaryDbContext, logger, mapper));
            _marketplaceService = new Lazy<IMarketplaceService>(() => new MarketplaceService(primaryDbContext, logger, mapper,tenantDbContext));
        }

        public IOrganizationService OrganizationService => _organizationService.Value;
        public ISystemSubscriptionService SystemSubscriptionService => _systemSubscriptionService.Value;
        public ISystemSubscriptionPlanService SystemSubscriptionPlanService => _systemSubscriptionPlanService.Value;
        public ITransactionService TransactionService => _transactionService.Value;
        public IBillService BillService => _billService.Value;
        public IMarketplaceService MarketplaceService => _marketplaceService.Value;
    }
}
