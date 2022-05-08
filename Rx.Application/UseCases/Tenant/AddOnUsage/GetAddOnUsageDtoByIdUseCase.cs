using AutoMapper;
using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOnUsage;

public record GetAddOnUsageDtoByIdUseCase(Guid SubscriptionId,Guid AddOnId,Guid AddOnUsageId):IRequest<AddOnUsageDto>;

public class GetAddOnUsageDtoByIdUseCaseHandler : IRequestHandler<GetAddOnUsageDtoByIdUseCase, AddOnUsageDto>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnUsageDtoByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<AddOnUsageDto> Handle(GetAddOnUsageDtoByIdUseCase request, CancellationToken cancellationToken)
    {
        var subscription = await _tenantDbContext.Subscriptions!.FindAsync(request.SubscriptionId);
        var addOn = await _tenantDbContext.AddOns!.FindAsync(request.AddOnId);
        if(subscription == null || addOn == null)
        {
            throw new Exception("Subscription or AddOn not found");
        }
        var addOnUsage = await _tenantDbContext.AddOnUsages!.FindAsync(request.AddOnUsageId);
        if(addOnUsage == null)
        {
            throw new Exception("AddOnUsage not found");
        }
        return _mapper.Map<AddOnUsageDto>(addOnUsage);
    }
}