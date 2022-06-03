using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOnUsage;

public record GetAddOnUsageDtosForSubscription(Guid SubscriptionId):IRequest<IEnumerable<AddOnUsageDto>>;

public class GetAddOnUsageDtosForSubscriptionHandler : IRequestHandler<GetAddOnUsageDtosForSubscription, IEnumerable<AddOnUsageDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnUsageDtosForSubscriptionHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AddOnUsageDto>> Handle(GetAddOnUsageDtosForSubscription request, CancellationToken cancellationToken)
    {
        var addOnUsages = await _tenantDbContext.AddOnUsages.Where(x => x.SubscriptionId == request.SubscriptionId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AddOnUsageDto>>(addOnUsages);
    }
}