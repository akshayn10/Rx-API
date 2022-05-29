using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnByIdUseCase(Guid ProductId,Guid AddOnId):IRequest<AddOnDto>;

public class GetAddOnByIdUseCaseHandler : IRequestHandler<GetAddOnByIdUseCase, AddOnDto>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<AddOnDto> Handle(GetAddOnByIdUseCase request, CancellationToken cancellationToken)
    {
        var product = await _tenantDbContext.Products!.FindAsync(request.ProductId);
        if (product == null)
            throw new Exception("Product not found");
        var addOn =await _tenantDbContext.AddOns!.Where(x=>x.ProductId==request.ProductId && x.AddOnId==request.AddOnId).FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<AddOnDto>(addOn);
    }
}