using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOnUsage;

public record CreateAddOnUsageUseCase(Guid SubscriptionId,Guid AddOnId,AddOnUsageForCreationDto AddOnUsageForCreationDto):IRequest<AddOnUsageDto>;

public class CreateAddOnUsageHandler : IRequestHandler<CreateAddOnUsageUseCase, AddOnUsageDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public CreateAddOnUsageHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<AddOnUsageDto> Handle(CreateAddOnUsageUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.AddOnUsageService.CreateAddOnUsage(request.SubscriptionId, request.AddOnId,request.AddOnUsageForCreationDto);
    }
}