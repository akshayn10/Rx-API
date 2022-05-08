using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;
public record GetSubscriptionsForCustomerDtoUseCase(Guid CustomerId):IRequest<IEnumerable<SubscriptionDto>>;

public class GetSubscriptionsForCustomerDtoUseCaseHandler : IRequestHandler<GetSubscriptionsForCustomerDtoUseCase, IEnumerable<SubscriptionDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;
    public GetSubscriptionsForCustomerDtoUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SubscriptionDto>> Handle(GetSubscriptionsForCustomerDtoUseCase request, CancellationToken cancellationToken)
    {
        var subscriptions = await _tenantDbContext.Subscriptions!.Where(x => x.OrganizationCustomerId == request.CustomerId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
    }
}