using System.ComponentModel.Design;
using AutoMapper;
using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Rx.Application.UseCases.Tenant.AddOn;
public record GetAddOnPerProductUseCase(Guid productId) : IRequest<IEnumerable<AddOnForProductVm>>;

public class GetAddOnPerProductUseCaseHandler : IRequestHandler<GetAddOnPerProductUseCase, IEnumerable<AddOnForProductVm>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetAddOnPerProductUseCaseHandler(ITenantDbContext tenantDbContext, IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AddOnForProductVm>> Handle(GetAddOnPerProductUseCase request,
        CancellationToken cancellationToken)
    {
        //var addOnVms = await _tenantDbContext.AddOns!.Where(a=>a.ProductId==request.productId).Include(=>a.AddOnPrices)
           // .Select(  )

           /*var addOn = await (from a in _tenantDbContext.AddOns
               join p in _tenantDbContext.AddOnPricePerPlans on a.AddOnId
                   equals p.AddOnId
               where a.ProductId == request.productId
               select new {a.Name, a.UnitOfMeasure, p.Price,a.AddOnId}).ToListAsync(cancellationToken);*/
           
           var x =await _tenantDbContext.AddOnPricePerPlans
               .Include(aop=>aop.AddOn).Include(aop=>aop.ProductPlan)
               .Where(aop=>aop.AddOn.ProductId==request.productId).ToListAsync(cancellationToken);

         //  var addOnVm = new AddOnForProductVm(
           //    Name: addOn.Name,
           //    UnitOfMeasure: addOn.UnitOfMeasure,
            //   Price: addOn.Price,  );

           return x.Select(a=>new AddOnForProductVm(
               Name: a.AddOn.Name,
               UnitOfMeasure: a.AddOn.UnitOfMeasure,
               Price: a.Price,
               PlanName:a.ProductPlan.Name,
               AddOnId:a.AddOn.AddOnId
               
           ));
    }
}