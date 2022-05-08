using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Billing;
using Rx.Domain.DTOs.Tenant.Bill;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[ApiController]
[Route("api/bill")]
public class BillController:ControllerBase
{
    private readonly IMediator _mediator;

    public BillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{subscriptionId}")]
    [SwaggerOperation(Summary = "Get All Bills")]
    public async Task<IActionResult> GetAllBills(string subscriptionId)
    {
        var subscriptionGuid = new Guid(subscriptionId);
        var bills =await _mediator.Send(new GetBillsUseCase(subscriptionGuid)); 
        return Ok(bills);
    }
    [HttpGet("{subscriptionId}/dtos")]
    [SwaggerOperation(Summary = "Get All Bills")]
    public async Task<IActionResult> GetAllBillDtos(string subscriptionId)
    {
        var subscriptionGuid = new Guid(subscriptionId);
        var bills =await _mediator.Send(new GetBillDtosUseCase(subscriptionGuid)); 
        return Ok(bills);
    }

    [HttpGet("{subscriptionId}/{billId}")]
    [SwaggerOperation(Summary = "Get a Bill by")]
    public async Task<IActionResult> GetBillById(string subscriptionId, string billId)
    {
        var subscriptionGuid = new Guid(subscriptionId);
        var billGuid = new Guid(billId);
        var bill = await _mediator.Send(new GetBillByIdUseCase(subscriptionGuid, billGuid));
        return Ok(bill);
    }

    [HttpPost("{subscriptionId}")]
    [SwaggerOperation(Summary = "Create a Bill")]
    public async Task<IActionResult> CreateBill(string subscriptionId, [FromBody] BillForCreationDto billForCreationDto)
    {
        var subscriptionGuid = new Guid(subscriptionId);
        var createdBill = await _mediator.Send(new CreateBillUseCase(subscriptionGuid, billForCreationDto));
        return CreatedAtAction(nameof(GetBillById), new { subscriptionId = subscriptionGuid, billId = createdBill.BillId }, createdBill);
    }



}