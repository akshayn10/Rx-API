using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Payment;

public record AddPaymentMethodForCustomerUseCase(string CustomerId,string Last4,string PaymentMethodId):IRequest<Guid>;

public class AddPaymentMethodForCustomerUseCaseHandler : IRequestHandler<AddPaymentMethodForCustomerUseCase,Guid>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddPaymentMethodForCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<Guid> Handle(AddPaymentMethodForCustomerUseCase request, CancellationToken cancellationToken)
    {
       return await _tenantServiceManager.OrganizationCustomerService.AddPaymentMethod(request.CustomerId, request.Last4,request.PaymentMethodId);
    }
}