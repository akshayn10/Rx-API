using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.Organization
{
    public record RetrieveOrganizationUseCase() : IRequest<IEnumerable<OrganizationDto>>;

    public class RetrieveOrganizationUseCaseHandler: IRequestHandler<RetrieveOrganizationUseCase, IEnumerable<OrganizationDto>>
    {
        private readonly IPrimaryServiceManager _primaryServiceManager;

        public RetrieveOrganizationUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
        {
            _primaryServiceManager = primaryServiceManager;
        }


        public async Task<IEnumerable<OrganizationDto>> Handle(RetrieveOrganizationUseCase request, CancellationToken cancellationToken)
        {
            var organizations = await _primaryServiceManager.OrganizationService.GetOrganizationsAsync(false);
            return organizations;

        }
    }
}
