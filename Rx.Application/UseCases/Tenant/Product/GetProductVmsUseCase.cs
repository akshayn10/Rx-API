using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Product;

public record GetProductVmsUseCase():IRequest<IEnumerable<ProductVm>>;

public class GetProductVmsUseCaseHandler : IRequestHandler<GetProductVmsUseCase,IEnumerable<ProductVm>>

{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetProductVmsUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ProductVm>> Handle(GetProductVmsUseCase request, CancellationToken cancellationToken)
    {
        var products = await _tenantDbContext.Products!.
            Select(p => new {p.ProductId,p.Name,p.RedirectURL,p.LogoURL}).ToListAsync(cancellationToken);
        
        var planCount =await _tenantDbContext.ProductPlans!.
            GroupBy(p=>p.ProductId).Select(group=>new {productId=group.Key,Count =group.Count()}).ToListAsync(cancellationToken);
        
        var addOnCount =await _tenantDbContext.AddOns!.
            GroupBy(a=>a.ProductId).Select(group=>new {productId=group.Key,Count=group.Count()}).ToListAsync(cancellationToken);

        var productVms = from p in products
            join pc in planCount on p.ProductId equals pc.productId into pcGroup 
            from pc in pcGroup.DefaultIfEmpty()
            join ac in addOnCount on p.ProductId equals ac.productId into acGroup 
            from ac in acGroup.DefaultIfEmpty()
            select (
                new ProductVm(
                    ProductId: p.ProductId.ToString(), Name: p.Name, RedirectUrl: p.RedirectURL, LogoURL: p.LogoURL,
                    PlanCount: pc?.Count??0, AddOnCount: ac?.Count??0
                ));
        return productVms;
    }
}