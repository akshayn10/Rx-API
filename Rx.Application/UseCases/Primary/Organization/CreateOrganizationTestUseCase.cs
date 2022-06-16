using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.Organization
{
    public record CreateOrganizationTestUseCase
        (OrganizationForCreationDto organizationForCreationDto) : IRequest<OrganizationDto>;

    public class CreateOrganizationTestUseCaseHandler : IRequestHandler<CreateOrganizationTestUseCase, OrganizationDto>
    {
        private readonly IPrimaryServiceManager _primaryServiceManager;

        public CreateOrganizationTestUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
        {
            _primaryServiceManager = primaryServiceManager;
            
        }
        public async Task<OrganizationDto> Handle(CreateOrganizationTestUseCase request, CancellationToken cancellationToken)
        {
            
            throw new NotImplementedException();
        }
    }
}
