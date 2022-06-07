using AutoMapper;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Entities.Tenant;
using Bill = Rx.Domain.Entities.Tenant.Bill;
using PaymentTransaction = Rx.Domain.Entities.Tenant.PaymentTransaction;


namespace Rx.API.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            //Organization
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationForCreationDto,Organization>();
            //OrganizationCustomer
            CreateMap<OrganizationCustomer, OrganizationCustomerDto>();
            CreateMap<OrganizationCustomerForCreationDto,OrganizationCustomer>();
            //Product
            CreateMap<Product,ProductDto>();
            CreateMap<ProductForCreationDto,Product>();
            //Subscription
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<SubscriptionForCreationDto, Subscription>();
            //ProductPlan
            CreateMap<ProductPlan, ProductPlanDto>();
            CreateMap<ProductPlanForCreationDto, ProductPlan>();
            //Billing
            CreateMap<Bill, BillDto>();
            CreateMap<BillForCreationDto, Bill>();
            //Transaction
            CreateMap<PaymentTransaction, TransactionDto>();
            CreateMap<TransactionForCreationDto, PaymentTransaction>();
            //AddOn
            CreateMap<AddOn, AddOnDto>();
            CreateMap<AddOnForCreationDto, AddOn>();
            //AddOnUsage
            CreateMap<AddOnUsage, AddOnUsageDto>();
            CreateMap<AddOnUsageForCreationDto, AddOnUsage>();
            //AddOnPricePerPlan
            CreateMap<AddOnPricePerPlan, AddOnPricePerPlanDto>();
            CreateMap<AddOnPricePerPlanForCreationDto, AddOnPricePerPlan>();
            //SubscriptionWebhook
            CreateMap<SubscriptionWebhookDto, SubscriptionWebhook>();
            CreateMap<SubscriptionWebhookForCreationDto, SubscriptionWebhook>();
            //AddOnWebhook
            CreateMap<AddOnWebhookDto, AddOnWebhook>();


        }
    }
}
