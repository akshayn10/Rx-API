using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Payment;
using Rx.Domain.Interfaces.Payment;
using Stripe;

namespace Rx.Domain.Services.Payment;

public class PaymentService:IPaymentService
{
    private readonly ILogger<PaymentService> _logger;
    public PaymentService(ILogger<PaymentService> logger,string apiKey)
    {
        _logger = logger;
        StripeConfiguration.ApiKey = apiKey;
    }
     public async Task<List<PaymentModel.CustomerModel>> GetCustomers(int take)
        {
            var service = new CustomerService();
            var stripeCustomers = await service.ListAsync(new CustomerListOptions()
            {
                Limit = take > 100 ? 100 : take,
            });

            return stripeCustomers.Select(x => new PaymentModel.CustomerModel(x.Id)
            {
                Email = x.Email,
                Name = x.Name,
                SystemId = x.Metadata["ID"]
            }).ToList();
        }

        public async Task<PaymentModel.CustomerModel> GetCustomerByEmail(string email, params PaymentModel.PaymentModelInclude[] includes)
        {
            var service = new CustomerService();
            var stripeCustomers = await service.ListAsync(new CustomerListOptions()
            {
                Email = email
            });

            if (!stripeCustomers.Any())
                return null;

            var stripeCustomer = stripeCustomers.Single();

            var customerModel = new PaymentModel.CustomerModel(stripeCustomer.Id)
            {
                Email = email,
                Name = stripeCustomer.Name
            };
            if (includes.Any() && includes.Contains(PaymentModel.PaymentModelInclude.PaymentMethods))
            {
                var paymentMethods = await this.GetPaymentMethods(stripeCustomer.Id, PaymentModel.PaymentMethodType.Card);
                customerModel.PaymentMethods = paymentMethods;
            }

            return customerModel;
        }

        public async Task<bool> CreateCustomer(string name, string email, string systemId)
        {
            this._logger.LogInformation("Creating Customer in Stripe");
            try
            {
                var options = new CustomerCreateOptions
                {
                    Email = email,
                    Name = name,
                    Metadata = new Dictionary<string, string>()
                    {
                        { "ID", systemId}
                    }
                };
                var service = new CustomerService();
                Customer c = await service.CreateAsync(options);
                this._logger.LogInformation("Customer Created succesfully");
                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogInformation($"An error occured during customer creation, {ex}");
                return false;
            }
        }

        public async Task<PaymentModel.CustomerModel> DeleteCustomerByEmail(string email)
        {
            var service = new CustomerService();
            var stripeCustomers = await service.ListAsync(new CustomerListOptions()
            {
                Email = email
            });

            var stripeCustomer = await GetCustomerByEmail(email);
            if (stripeCustomer == null) return null;

            var deletedStripeCustomer = await service.DeleteAsync(stripeCustomer.Id);
            return new PaymentModel.CustomerModel(deletedStripeCustomer.Id)
            {
                Name = deletedStripeCustomer.Name,
                Email = deletedStripeCustomer.Email,
                SystemId = deletedStripeCustomer.Metadata?.GetValueOrDefault("ID")
            };
        }


        public async Task<PaymentModel.PaymentMethodModel> AttachPaymentMethod(string paymentMethodId, string customerId, bool makeDefault = true)
        {
            try
            {
                var options = new PaymentMethodAttachOptions
                {
                    Customer = customerId,
                };
                var service = new PaymentMethodService();
                var stripePaymentMethod = await service.AttachAsync(paymentMethodId, options);

                if (makeDefault)
                {
                    // Update customer's default invoice payment method
                    var customerOptions = new CustomerUpdateOptions
                    {
                        InvoiceSettings = new CustomerInvoiceSettingsOptions
                        {
                            DefaultPaymentMethod = stripePaymentMethod.Id,
                        },
                    };
                    var customerService = new CustomerService();
                    await customerService.UpdateAsync(customerId, customerOptions);
                }

                PaymentModel.PaymentMethodModel result = new PaymentModel.PaymentMethodModel(stripePaymentMethod.Id);

                if (!Enum.TryParse(stripePaymentMethod.Type, true, out PaymentModel.PaymentMethodType paymentMethodType))
                {
                    this._logger.LogError($"Cannot recognize PAYMENT_METHOD_TYPE:{stripePaymentMethod.Type}");
                }
                result.Type = paymentMethodType;

                if (result.Type == PaymentModel.PaymentMethodType.Card)
                {
                    result.Card = new PaymentModel.PaymentMethodCardModel()
                    {
                        Brand = stripePaymentMethod.Card.Brand,
                        Country = stripePaymentMethod.Card.Country,
                        ExpMonth = stripePaymentMethod.Card.ExpMonth,
                        ExpYear = stripePaymentMethod.Card.ExpYear,
                        Issuer = stripePaymentMethod.Card.Issuer,
                        Last4 = stripePaymentMethod.Card.Last4,
                        Description = stripePaymentMethod.Card.Description,
                        Fingerprint = stripePaymentMethod.Card.Fingerprint,
                        Funding = stripePaymentMethod.Card.Funding,
                        Iin = stripePaymentMethod.Card.Iin
                    };
                }

                return result;
            }
            catch (StripeException se)
            {
                this._logger.LogError($"An error occured during attach of PAYMENT_METHOD:{paymentMethodId} for CUSTOMER:{customerId}, {se}");
            }
            catch (Exception ex)
            {
                this._logger.LogError($"An error occured during attach of PAYMENT_METHOD:{paymentMethodId} for CUSTOMER:{customerId}, {ex}");
            }
            return null;
        }

        public async Task DeletePaymentMethod(string paymentMethodId)
        {
            var service = new PaymentMethodService();
            var paymentMethod = await service.DetachAsync(paymentMethodId);

        }



        public async Task<PaymentModel.FuturePaymentIntentModel> PrepareForFuturePaymentWithCustomerEmail(string customerEmail)
        {
            var stripeCustomer = await this.GetCustomerByEmail(customerEmail);
            if (stripeCustomer == null)
                return null;

            PaymentModel.FuturePaymentIntentModel intent = await PrepareForFuturePayment(stripeCustomer.Id);
            return intent;
        }

        public async Task<PaymentModel.FuturePaymentIntentModel> PrepareForFuturePayment(string customerId)
        {
            var options = new SetupIntentCreateOptions
            {
                Customer = customerId,
                Expand = new List<string>()
                {
                    "customer"
                }
            };

            var service = new SetupIntentService();
            var intent = await service.CreateAsync(options);
            return new PaymentModel.FuturePaymentIntentModel()
            {
                Id = intent.Id,
                IntentSecret = intent.ClientSecret,
                Customer = new PaymentModel.CustomerModel(intent.Customer.Id)
                {
                    Email = intent.Customer.Email,
                    Name = intent.Customer.Name,
                    SystemId = intent.Customer.Metadata?.GetValueOrDefault("ID"),
                }
            };
        }


        public async Task<List<PaymentModel.PaymentMethodModel>> GetPaymentMethods(string customerId, PaymentModel.PaymentMethodType paymentMethodType)
        {
            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = paymentMethodType.ToString().ToLower()
            };

            var service = new PaymentMethodService();
            var paymentMethods = await service.ListAsync(options);


            List<PaymentModel.PaymentMethodModel> result = new List<PaymentModel.PaymentMethodModel>();
            foreach (var stripePaymentMethod in paymentMethods)
            {
                if (!Enum.TryParse(stripePaymentMethod.Type, true, out PaymentModel.PaymentMethodType currPaymentMethodType))
                {
                    this._logger.LogError($"Cannot recognize PAYMENT_METHOD_TYPE:{stripePaymentMethod.Type}");
                    continue;
                }

                PaymentModel.PaymentMethodModel currentPaymentMethod = new PaymentModel.PaymentMethodModel(stripePaymentMethod.Id)
                {
                    Type = currPaymentMethodType
                };

                if (currPaymentMethodType == PaymentModel.PaymentMethodType.Card)
                {
                    currentPaymentMethod.Card = new PaymentModel.PaymentMethodCardModel()
                    {
                        Brand = stripePaymentMethod.Card.Brand,
                        Country = stripePaymentMethod.Card.Country,
                        ExpMonth = stripePaymentMethod.Card.ExpMonth,
                        ExpYear = stripePaymentMethod.Card.ExpYear,
                        Issuer = stripePaymentMethod.Card.Issuer,
                        Last4 = stripePaymentMethod.Card.Last4,
                        Description = stripePaymentMethod.Card.Description,
                        Fingerprint = stripePaymentMethod.Card.Fingerprint,
                        Funding = stripePaymentMethod.Card.Funding,
                        Iin = stripePaymentMethod.Card.Iin
                    };
                }

                result.Add(currentPaymentMethod);
            }
            return result;
        }

        public async Task<List<PaymentModel.PaymentMethodModel>> GetPaymentMethodsByCustomerEmail(string customerEmail, PaymentModel.PaymentMethodType paymentMethodType)
        {
            PaymentModel.CustomerModel customer = await this.GetCustomerByEmail(customerEmail);

            return await this.GetPaymentMethods(customer.Id, paymentMethodType);
        }
        
        public async Task ChargeWithCustomerEmail(string customerEmail, string paymentMethodId, PaymentModel.Currency currency, long unitAmount,
            bool sendEmailAfterSuccess = false, string emailDescription = "")
        {
            var customer = await GetCustomerByEmail(customerEmail);
            await Charge(customer.Id, paymentMethodId, currency, unitAmount, customerEmail, sendEmailAfterSuccess, emailDescription);
        }

        // customize receipt -> https://dashboard.stripe.com/settings/branding
        // -> https://dashboard.stripe.com/settings/billing/invoice
        // in case of email send uppon failure -> https://dashboard.stripe.com/settings/billing/automatic
        public async Task Charge(string customerId, string paymentMethodId,
            PaymentModel.Currency currency, long unitAmount, string customerEmail, bool sendEmailAfterSuccess = false, string emailDescription = "")
        {
            try
            {
                var service = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = unitAmount,
                    Currency = currency.ToString().ToLower(),
                    Customer = customerId,
                    PaymentMethod = paymentMethodId,
                    Confirm = true,
                    OffSession = true,
                    ReceiptEmail = sendEmailAfterSuccess ? customerEmail : null,
                    Description = emailDescription,
                };
                await service.CreateAsync(options);
            }
            catch (StripeException e)
            {
                switch (e.StripeError.Type)
                {
                    case "card_error":
                        // Error code will be authentication_required if authentication is needed
                        Console.WriteLine("Error code: " + e.StripeError.Code);
                        var paymentIntentId = e.StripeError.PaymentIntent.Id;
                        var service = new PaymentIntentService();
                        var paymentIntent = service.Get(paymentIntentId);

                        Console.WriteLine(paymentIntent.Id);
                        break;
                    default:
                        break;
                }
            }
        }


        public async Task<IEnumerable<PaymentModel.ChargeModel>> GetPaymentStatus(string paymentId)
        {
            var service = new PaymentIntentService();
            var intent = await service.GetAsync(paymentId);
            var charges = intent.Charges.Data;

            return charges.Select(x => new PaymentModel.ChargeModel(x.Id)
            {
                Status = x.Status
            });
        }
    
}