using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnPricePerPlanByIdUseCase(Guid AddOnId,Guid PlanId, Guid AddOnPricePerPlanId):IRequest<AddOnPricePerPlanDto>;

public class GetAddOnPricePerPlanByIdUseCaseHandler : IRequestHandler<GetAddOnPricePerPlanByIdUseCase, AddOnPricePerPlanDto>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnPricePerPlanByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<AddOnPricePerPlanDto> Handle(GetAddOnPricePerPlanByIdUseCase request, CancellationToken cancellationToken)
    {
        var product = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(x => x.PlanId == request.PlanId, cancellationToken: cancellationToken);
        var addOn = await _tenantDbContext.AddOns!.FirstOrDefaultAsync(x => x.AddOnId == request.AddOnId, cancellationToken: cancellationToken);
        if(product == null || addOn == null)
        {
            throw new Exception("Product or AddOn not found");
        }
        
        var addOnPricePerPlan = await _tenantDbContext.AddOnPricePerPlans!.FirstOrDefaultAsync(x => x.AddOnPricePerPlanId == request.AddOnPricePerPlanId, cancellationToken: cancellationToken);
        return _mapper.Map<AddOnPricePerPlanDto>(addOnPricePerPlan);

    }
}

