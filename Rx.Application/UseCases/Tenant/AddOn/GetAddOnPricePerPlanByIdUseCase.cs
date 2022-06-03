using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnPricePerPlanByIdUseCase(Guid AddOnPricePerPlanId):IRequest<AddOnPricePerPlanDto>;

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
        var addOnPricePerPlan = await _tenantDbContext.AddOnPricePerPlans!.FindAsync(request.AddOnPricePerPlanId);
        return _mapper.Map<AddOnPricePerPlanDto>(addOnPricePerPlan);

    }
}

