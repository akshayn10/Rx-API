using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnPricePerPlanUseCase(Guid AddOnId):IRequest<IEnumerable<AddOnPricePerPlanDto>>;

public class GetAddOnPricePerPlanUseCaseHandler : IRequestHandler<GetAddOnPricePerPlanUseCase, IEnumerable<AddOnPricePerPlanDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnPricePerPlanUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AddOnPricePerPlanDto>> Handle(GetAddOnPricePerPlanUseCase request, CancellationToken cancellationToken)
    {
        var addOn = await _tenantDbContext.AddOns!.FindAsync(request.AddOnId);
        if (addOn == null)
            throw new Exception("AddOn not found");
        var addOnPricePerPlans = await _tenantDbContext.AddOnPricePerPlans!.Where(x => x.AddOnId == request.AddOnId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AddOnPricePerPlanDto>>(addOnPricePerPlans);
    }
}