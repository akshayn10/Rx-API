using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Payment;

public record AddPaymentGatewayIdToCustomerUseCase(Guid CustomerId, string PaymentGatewayId):IRequest;

public class AddPaymentGatewayIdToCustomerUseCaseHandler : IRequestHandler<AddPaymentGatewayIdToCustomerUseCase>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddPaymentGatewayIdToCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<Unit> Handle(AddPaymentGatewayIdToCustomerUseCase request, CancellationToken cancellationToken)
    {
        await _tenantServiceManager.OrganizationCustomerService.AddPaymentGatewayIdToCustomer(request.CustomerId, request.PaymentGatewayId);
        return Unit.Value;
    }
}