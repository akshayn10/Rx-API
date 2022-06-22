using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.Organization;

public record EditOrganizationUseCase(Guid OrganizationId,EditOrganizationRequestDto EditOrganizationRequestDto):IRequest<OrganizationDto>;
public class EditOrganizationUseCaseHandler : IRequestHandler<EditOrganizationUseCase, OrganizationDto>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public EditOrganizationUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<OrganizationDto> Handle(EditOrganizationUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.OrganizationService.EditOrganization(request.OrganizationId,request.EditOrganizationRequestDto);
    }
}