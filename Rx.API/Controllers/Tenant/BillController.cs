using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Billing;
using Rx.Domain.DTOs.Tenant.Bill;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[Authorize(Roles = "Owner")]
[ApiController]
[Route("api/bill")]
public class BillController:ControllerBase
{
    private readonly IMediator _mediator;

    public BillController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get All Bills")]
    public async Task<IActionResult> GetAllBills()
    {
        var bills =await _mediator.Send(new GetBillsUseCase()); 
        return Ok(bills);
    }
    [HttpGet("dtos")]
    [SwaggerOperation(Summary = "Get All Bills dtos")]
    public async Task<IActionResult> GetAllBillDtos()
    {
        var bills =await _mediator.Send(new GetBillDtosUseCase()); 
        return Ok(bills);
    }
    [HttpGet("customer/{customerId}")]
    [SwaggerOperation(Summary = "Get All Bill by customerId")]
    public async Task<IActionResult> GetAllBillByCustomerId(string customerId)
    {
        var bill =await _mediator.Send(new GetBillByCustomerIdUseCase(Guid.Parse(customerId))); 
        return Ok(bill);
    }

    [HttpGet("{billId}")]
    [SwaggerOperation(Summary = "Get a Bill by")]
    public async Task<IActionResult> GetBillById(string billId)
    {
        var bill = await _mediator.Send(new GetBillByIdUseCase(Guid.Parse(billId)));
        return Ok(bill);
    }

    [HttpPost("{customerId}")]
    [SwaggerOperation(Summary = "Create a Bill")]
    public async Task<IActionResult> CreateBill(string customerId, [FromBody] BillForCreationDto billForCreationDto)
    {
        var customerGuid = new Guid(customerId);
        var createdBill = await _mediator.Send(new CreateBillUseCase(customerGuid, billForCreationDto));
        return CreatedAtAction(nameof(GetBillById), new { billId = createdBill.BillId }, createdBill);
    }



}

