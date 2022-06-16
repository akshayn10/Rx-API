using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.Organization;

public record CreateOrganizationUseCase(CreateOrganizationRequestDto CreateOrganizationRequestDto):IRequest<Guid>;

public class CreateOrganizationUseCaseHandler : IRequestHandler<CreateOrganizationUseCase, Guid>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public CreateOrganizationUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<Guid> Handle(CreateOrganizationUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.OrganizationService.CreateOrganizationAsync(request
            .CreateOrganizationRequestDto);
    }
}