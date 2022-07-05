using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant
{
    public class BillingService : IBillingService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BillingService> _logger;

        public BillingService(ITenantDbContext tenantDbContext,ILogger<BillingService> logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task GenerateBill(Guid customerId)
        {
            var customerIsActive = _tenantDbContext.Subscriptions!.Any(x => x.OrganizationCustomerId == customerId && x.IsActive);
            if (customerIsActive)
            {
                var customer = await _tenantDbContext.OrganizationCustomers!.FindAsync(customerId);
                var oneMonthAgo = DateTime.Now.AddMonths(-1);
                var subscriptionsForCustomer = await _tenantDbContext.Subscriptions!
                    .Where(s => s.OrganizationCustomerId == customerId && s.IsActive).Include(a => a.AddOnUsages)
                    .Select(s =>
                        new SubscriptionForBill(
                            s.SubscriptionId.ToString(),
                            s.StartDate.ToShortDateString()+" "+ s.StartDate.ToLongTimeString(),
                            s.ProductPlan.Product.Name,
                            s.ProductPlan.Name,
                            s.SubscriptionType?"Recurring":"One Time",
                            s.StartDate>oneMonthAgo?s.ProductPlan.Price:0,
                            s.AddOnUsages.Where(aou=>aou.Date>oneMonthAgo).Select(a =>
                                new AddOnForSubscription(
                                    a.Date.ToShortDateString()+" "+a.Date.ToLongTimeString(),
                                    a.AddOn.Name,
                                    a.Unit,
                                    a.TotalAmount / a.Unit,
                                    a.TotalAmount
                                )
                            )
                        )
                    ).ToListAsync();
                decimal totalAmount = 0;
                
                foreach (var s in subscriptionsForCustomer)
                {
                    totalAmount += s.Price;
                    foreach (var ao in s.AddOnsForSubscription )
                    {
                        totalAmount += ao.TotalAmount;
                    }
                }
                var subscriptionForBillJson = JsonConvert.SerializeObject(subscriptionsForCustomer);

                var billForCreation = new BillForCreationDto(
                    DateTime.Now,
                    totalAmount,
                    customerId,
                    subscriptionForBillJson);
                
                var bill = _mapper.Map<Bill>(billForCreation);
                await _tenantDbContext.Bills.AddAsync(bill);
                await _tenantDbContext.SaveChangesAsync();
                
            }

        }
        

        public async Task<BillVm> GetBillByCustomerId(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<BillVm> GetBillByBillId(Guid billId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BillVm>> GetBillsByCustomerId(Guid customerId)
        {
            throw new NotImplementedException();
        }
    }
}
