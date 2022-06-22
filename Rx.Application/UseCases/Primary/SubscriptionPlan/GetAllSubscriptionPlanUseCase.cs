using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Primary.SystemSubscriptionPlan;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Primary.SubscriptionPlan;

public record GetAllSubscriptionPlanUseCase():IRequest<IEnumerable<SystemSubscriptionPlanDto>>;

public class GetAllSubscriptionPlanUseCaseHandler : IRequestHandler<GetAllSubscriptionPlanUseCase,IEnumerable<SystemSubscriptionPlanDto>>
{
    private readonly IPrimaryDbContext _primaryDbContext;

    public GetAllSubscriptionPlanUseCaseHandler(IPrimaryDbContext primaryDbContext)
    {
        _primaryDbContext = primaryDbContext;
    }
    public async Task<IEnumerable<SystemSubscriptionPlanDto>> Handle(GetAllSubscriptionPlanUseCase request, CancellationToken cancellationToken)
    {
        var plans=  await _primaryDbContext.SystemSubscriptionPlans.OrderBy(p=>p.Price).Select(s=>
                new SystemSubscriptionPlanDto(
                    s.PlanId.ToString(),
                    s.Name!,
                    s.Description!,
                    s.Price,
                    s.Duration
                    )
                )
            .ToListAsync(cancellationToken: cancellationToken);
        return plans;
    }
}