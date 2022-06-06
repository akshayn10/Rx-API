using AutoMapper;
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

        public OrganizationCustomerService(ITenantDbContext tenantDbContext,ILogger logger, IMapper mapper,IPaymentService paymentService)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
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

            return customer!.CustomerId;
        }
        
        public async Task<string> CreateCustomerFromWebhook(SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {

                var customerForCreationDto = new OrganizationCustomerForCreationDto(
                    Email: subscriptionWebhookForCreationDto.customerEmail,
                    Name: subscriptionWebhookForCreationDto.customerName
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
