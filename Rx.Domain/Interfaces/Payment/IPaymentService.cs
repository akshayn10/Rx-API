using Rx.Domain.DTOs.Payment;

namespace Rx.Domain.Interfaces.Payment;

public interface IPaymentService
{
    Task<bool> CreateCustomer(string name, string email, string systemId);
    Task<List<PaymentModel.CustomerModel>> GetCustomers(int take);
    Task<PaymentModel.CustomerModel> GetCustomerByEmail(string email, params PaymentModel.PaymentModelInclude[] include);
    
    Task<string> GetCustomerEmailById(string id);
    Task<PaymentModel.CustomerModel> DeleteCustomerByEmail(string email);

    /// <summary>
    /// when you want to add a payment method for future payment for this particular customer.
    /// Use the return object depending depending the payment provider, e.g. for stripe use the IntentSecret as the ClientSecret
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task<PaymentModel.FuturePaymentIntentModel> PrepareForFuturePayment(string customerId);
    Task<PaymentModel.FuturePaymentIntentModel> PrepareForFuturePaymentWithCustomerEmail(string customerEmail);
    Task<List<PaymentModel.PaymentMethodModel>> GetPaymentMethods(string customerId, PaymentModel.PaymentMethodType paymentMethodType);
    Task<List<PaymentModel.PaymentMethodModel>> GetPaymentMethodsByCustomerEmail(string customerEmail, PaymentModel.PaymentMethodType paymentMethodType);
    Task DeletePaymentMethod(string paymentMethodId);

    Task<string> Charge(string customerId, string paymentMethodId, PaymentModel.Currency currency, long unitAmount,
        string customerEmail, bool sendEmailAfterSuccess , string chargeDescription);

    Task ChargeWithCustomerEmail(string customerEmail, string paymentMethodId, PaymentModel.Currency currency, long unitAmount,
        bool sendEmailAfterSuccess = true, string emailDescription = "");

    Task<IEnumerable<PaymentModel.ChargeModel>> GetPaymentStatus(string paymentId);
}