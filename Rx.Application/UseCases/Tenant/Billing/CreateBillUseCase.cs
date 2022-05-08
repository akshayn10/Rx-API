using MediatR;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Billing;

public record CreateBillUseCase(Guid SubscriptionId,BillForCreationDto BillForCreationDto):IRequest<BillDto>;

public class CreateBillUseCaseHandler : IRequestHandler<CreateBillUseCase, BillDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public CreateBillUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<BillDto> Handle(CreateBillUseCase request, CancellationToken cancellationToken)
    {
        var bill = await _tenantServiceManager.BillingService.CreateBill(request.SubscriptionId, request.BillForCreationDto);
        return bill;
    }
}