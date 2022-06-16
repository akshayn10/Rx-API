using MediatR;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Application.UseCases.Tenant.Billing;

public record CreateBillUseCase(Guid CustomerId,BillForCreationDto BillForCreationDto):IRequest<BillDto>;

public class CreateBillUseCaseHandler : IRequestHandler<CreateBillUseCase, BillDto>
{
    private readonly IBillingService _billingService;

    public CreateBillUseCaseHandler(IBillingService billingService)
    {
        _billingService = billingService;
    }
    public async Task<BillDto> Handle(CreateBillUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}