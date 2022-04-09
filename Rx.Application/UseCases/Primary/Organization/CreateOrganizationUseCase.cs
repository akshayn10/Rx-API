using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.Organization
{
    public record CreateOrganizationUseCase
        (OrganizationForCreationDto organizationForCreationDto) : IRequest<OrganizationDto>;

    public class CreateOrganizationUseCaseHandler : IRequestHandler<CreateOrganizationUseCase, OrganizationDto>
    {
        private readonly IPrimaryServiceManager _primaryServiceManager;

        public CreateOrganizationUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
        {
            _primaryServiceManager = primaryServiceManager;
            
        }
        public async Task<OrganizationDto> Handle(CreateOrganizationUseCase request, CancellationToken cancellationToken)
        {
            var createdOrganizationDto = await _primaryServiceManager.OrganizationService.CreateOrganizationAsync(request.organizationForCreationDto);
            return createdOrganizationDto;
        }
    }
}
