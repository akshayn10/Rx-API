using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Primary.Organization;

public record GetOrganizationSubscriptionStatusUseCase(Guid OrganizationId):IRequest<bool>;
public class GetOrganizationSubscriptionStatusUseCaseHandler:IRequestHandler<GetOrganizationSubscriptionStatusUseCase,bool>
{
    private readonly IPrimaryDbContext _primaryDbContext;

    public GetOrganizationSubscriptionStatusUseCaseHandler(IPrimaryDbContext primaryDbContext)
    {
        _primaryDbContext = primaryDbContext;
    }
    public async Task<bool> Handle(GetOrganizationSubscriptionStatusUseCase request, CancellationToken cancellationToken)
    {
        var status = _primaryDbContext.SystemSubscriptions!.Any(s =>s.IsActive && s.OrganizationId == request.OrganizationId);
        return status;

    }
}