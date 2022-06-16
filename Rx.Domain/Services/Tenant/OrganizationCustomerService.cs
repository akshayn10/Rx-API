using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Tenant;


namespace Rx.Domain.Services.Tenant
{
    public class OrganizationCustomerService : IOrganizationCustomerService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IBillingService _billingService;
        private readonly IRecurringJobManager _recurringJobManager;

        public OrganizationCustomerService(ITenantDbContext tenantDbContext,
            ILogger logger,
            IMapper mapper,
            IPaymentService paymentService,
            IBillingService billingService,
            IRecurringJobManager recurringJobManager
            )
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
            _billingService = billingService;
            _recurringJobManager = recurringJobManager;
        }

        public async Task<IEnumerable<OrganizationCustomerDto>> GetCustomers()
        {
            var customers =await _tenantDbContext.OrganizationCustomers!.ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationCustomerDto>>(customers);
        }

        public async Task<OrganizationCustomerDto> GetCustomerById(Guid id)
        {
            var customer = await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(x => x.CustomerId == id);

            return _mapper.Map<OrganizationCustomerDto>(customer);
        }

        public async Task<OrganizationCustomerDto> AddCustomer(OrganizationCustomerForCreationDto organizationCustomerForCreationDto)
        {
            var customer = _mapper.Map<OrganizationCustomer>(organizationCustomerForCreationDto);
            await _tenantDbContext.OrganizationCustomers!.AddAsync(customer);
            await _tenantDbContext.SaveChangesAsync();

            return _mapper.Map<OrganizationCustomerDto>(customer);
        }

        public async Task<Guid> AddPaymentMethod(string customerId, string last4,string paymentMethodId)
        {
            var customerEmail=await _paymentService.GetCustomerEmailById(customerId);
            var customer = await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(c=>c.Email==customerEmail);
            if (customer == null)
            {
                throw new NullReferenceException("Customer not found for the email");
            }
            customer.PaymentGatewayId = customerId;
            customer.Last4 = last4;
            customer.PaymentMethodId = paymentMethodId;
            await _tenantDbContext.SaveChangesAsync();
            var addOnWebhook =await _tenantDbContext.SubscriptionWebhooks.Where(sw => sw.CustomerEmail == customerEmail)
                .OrderByDescending(s=>s.RetrievedDate)
                .FirstOrDefaultAsync();
            
            
            //Create a recurring job to start Bill Generation
            var billJobId = "bill_" + customer.CustomerId.ToString();
            _recurringJobManager.AddOrUpdate(billJobId,() => _billingService.GenerateBill(customer.CustomerId),
                Cron.Monthly(DateTime.Now.Day));

            return addOnWebhook!.WebhookId;
        }
        
        public async Task<string> CreateCustomerFromWebhook(Guid webhookId)
        {
            var webhook =await _tenantDbContext.SubscriptionWebhooks.FindAsync(webhookId);

                var customerForCreationDto = new OrganizationCustomerForCreationDto(
                    Email: webhook!.CustomerEmail!,
                    Name: webhook.CustomerName!
                );
                //Create New Customer
                var customer = _mapper.Map<OrganizationCustomer>(customerForCreationDto);
                await _tenantDbContext.OrganizationCustomers!.AddAsync(customer);
                await _tenantDbContext.SaveChangesAsync();
                await _paymentService.CreateCustomer(customer.Name!, customer.Email!,customer.CustomerId.ToString());
                return "https://localhost:44352/api/Payment?customerEmail="+customer.Email;;
        }

        public async Task<CustomerStatsDto> GetCustomerStats()
        {
            var totalCustomer =await _tenantDbContext.OrganizationCustomers!.CountAsync();
            var totalActiveCustomer = await (from c in _tenantDbContext.OrganizationCustomers
                join s in _tenantDbContext.Subscriptions on c.CustomerId equals s.OrganizationCustomerId
                where s.IsActive == true
                select c).Distinct().CountAsync();
            CustomerStatsDto customerStatsDto = new CustomerStatsDto(totalCustomer, totalActiveCustomer);
            return customerStatsDto;
        }
    }
}
