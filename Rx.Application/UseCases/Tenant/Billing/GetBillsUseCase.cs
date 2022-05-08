using AutoMapper;
using MediatR;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillsUseCase(Guid SubscriptionId):IRequest<IEnumerable<BillDto>>;

public class GetBillsUseCaseHandler : IRequestHandler<GetBillsUseCase, IEnumerable<BillDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetBillsUseCaseHandler(ITenantDbContext tenantDbContext, IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public Task<IEnumerable<BillDto>> Handle(GetBillsUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}