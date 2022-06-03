using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;


namespace Rx.Application.UseCases.Tenant.Webhook;

public record GetWebhookSecretUseCase(Guid ProductId) : IRequest<string>;

public class GetWebhookSecretUseCaseHandler:IRequestHandler<GetWebhookSecretUseCase,string>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetWebhookSecretUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }

    public async Task<string> Handle(GetWebhookSecretUseCase request, CancellationToken cancellationToken)
    {
        var webhookSecret = await _tenantDbContext.Products!.Where(p=>p.ProductId==request.ProductId)
            .Select(p=>p.WebhookSecret).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return webhookSecret ?? throw new InvalidOperationException("Product Not Found");
    }
}
