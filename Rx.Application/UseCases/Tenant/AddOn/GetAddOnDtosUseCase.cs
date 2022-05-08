using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnDtosUseCase(Guid ProductId):IRequest<IEnumerable<AddOnDto>>;

public class GetAddOnDtosUseCaseHandler : IRequestHandler<GetAddOnDtosUseCase, IEnumerable<AddOnDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnDtosUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AddOnDto>> Handle(GetAddOnDtosUseCase request, CancellationToken cancellationToken)
    {
        var addOns =await _tenantDbContext.AddOns!.Where(a=>a.ProductId==request.ProductId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AddOnDto>>(addOns);
    }
}