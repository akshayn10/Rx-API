using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionsDtoUseCase():IRequest<IEnumerable<SubscriptionDto>>;

public class GetSubscriptionDtoUseCaseHandler : IRequestHandler<GetSubscriptionsDtoUseCase, IEnumerable<SubscriptionDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetSubscriptionDtoUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SubscriptionDto>> Handle(GetSubscriptionsDtoUseCase request, CancellationToken cancellationToken)
    {
        var subscriptions =await _tenantDbContext.Subscriptions!.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
    }
}