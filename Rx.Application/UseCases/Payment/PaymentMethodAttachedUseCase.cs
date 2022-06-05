using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Payment;

public record PaymentMethodAttachedUseCase(string CustomerId,string Last4):IRequest<Guid>;

public class PaymentMethodAttachedUseCaseHandler : IRequestHandler<PaymentMethodAttachedUseCase,Guid>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public PaymentMethodAttachedUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<Guid> Handle(PaymentMethodAttachedUseCase request, CancellationToken cancellationToken)
    {
       return await _tenantServiceManager.OrganizationCustomerService.AddPaymentMethod(request.CustomerId, request.Last4);
    }
}