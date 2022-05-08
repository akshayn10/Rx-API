using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOnUsage;

public record GetAddOnUsageDtosForAddOn(Guid AddOnId):IRequest<IEnumerable<AddOnUsageDto>>;

public class GetAddOnUsageDtosForAddOnHandler : IRequestHandler<GetAddOnUsageDtosForAddOn, IEnumerable<AddOnUsageDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnUsageDtosForAddOnHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AddOnUsageDto>> Handle(GetAddOnUsageDtosForAddOn request, CancellationToken cancellationToken)
    {
        var addOnUsages = await _tenantDbContext.AddOnUsages.Where(x => x.AddOnId == request.AddOnId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AddOnUsageDto>>(addOnUsages);
    }
}