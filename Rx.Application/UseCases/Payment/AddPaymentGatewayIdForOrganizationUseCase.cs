using MediatR;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.Payment;

public record AddPaymentGatewayIdForOrganizationUseCase(Guid OrganizationId, string PaymentGatewayId):IRequest;

public class AddPaymentGatewayIdForOrganizationUseCaseHandler : IRequestHandler<AddPaymentGatewayIdForOrganizationUseCase>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public AddPaymentGatewayIdForOrganizationUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<Unit> Handle(AddPaymentGatewayIdForOrganizationUseCase request, CancellationToken cancellationToken)
    {
        await _primaryServiceManager.OrganizationService.AddPaymentGatewayIdForOrganization(request.OrganizationId,request.PaymentGatewayId);
        return Unit.Value;
    }
}