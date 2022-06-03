﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillByIdUseCase(Guid BillId):IRequest<BillDto>;

public class GetBillByIdUseCaseHandler : IRequestHandler<GetBillByIdUseCase, BillDto>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetBillByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<BillDto> Handle(GetBillByIdUseCase request, CancellationToken cancellationToken)
    {
        var bill =await _tenantDbContext.Bills.FirstOrDefaultAsync(x=>x.BillId==request.BillId);
        return _mapper.Map<BillDto>(bill);
    }
}