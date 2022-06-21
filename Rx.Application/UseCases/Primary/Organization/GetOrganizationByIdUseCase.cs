using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Primary.Organization;

public record GetOrganizationByIdUseCase(Guid OrganizationId):IRequest<OrganizationVm>;

public class GetOrganizationByIdUseCaseHandler : IRequestHandler<GetOrganizationByIdUseCase, OrganizationVm>
{
    private readonly IPrimaryDbContext _primaryDbContext;

    public GetOrganizationByIdUseCaseHandler(IPrimaryDbContext primaryDbContext)
    {
        _primaryDbContext = primaryDbContext;
    }
    public async Task<OrganizationVm> Handle(GetOrganizationByIdUseCase request, CancellationToken cancellationToken)
    {
        var organization =  await _primaryDbContext.Organizations!.FindAsync(request.OrganizationId);
        var address = await _primaryDbContext.OrganizationAddresses!.FirstOrDefaultAsync(a=>a.OrganizationId == request.OrganizationId, cancellationToken: cancellationToken);
        return new OrganizationVm(
        OrganizationId:organization!.Id.ToString(),
        Name:organization.Name,
        Description:organization.Description,
        LogoUrl:organization.LogoURL,
        Email:organization.Email,
        AccountOwnerId:organization.AccountOwnerId,
        new OrganizationAddressVm(
            address!.AddressLine1,
            address.AddressLine2,
             address.City,
             address.State,
             address.Country)
        );

    }
}
