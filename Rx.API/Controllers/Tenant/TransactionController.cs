using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Transaction;
using Rx.Domain.DTOs.Tenant.Transaction;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[ApiController]
[Route("api/transaction")]
public class TransactionController:ControllerBase
{

    private readonly IMediator _mediator;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(IMediator mediator, ILogger<TransactionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions =await _mediator.Send(new GetTransactionsUseCase());
        return Ok(transactions);
    }

    [HttpGet("{transactionId}")]
    [SwaggerOperation(Summary = "Get transaction by id")]
    public async Task<IActionResult> GetTransactionById(Guid transactionId)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdUseCase(transactionId));
        return Ok(transaction);
    }
    [HttpPost("{billId}")]
    [SwaggerOperation(Summary = "Create transaction")]
    public async Task<IActionResult> CreateTransaction(string billId,[FromBody] TransactionForCreationDto transactionForCreationDto)
    {
        var billGuid = new Guid(billId);
        var transaction = await _mediator.Send(new CreateTransactionUseCase(billGuid,transactionForCreationDto));
        return Ok(transaction);
    }
    
    [HttpGet("/customer/{customerId}")]
    [SwaggerOperation(Summary = "Get transactions by customer id")]
    public async Task<IActionResult> GetTransactionsByCustomerId(string customerId)
    {
        var customerGuid = new Guid(customerId);
        var transactions = await _mediator.Send(new GetTransactionsByCustomerIdUseCase(customerGuid));
        return Ok(transactions);
    }

    [HttpGet("/subscription/{subscriptionId}")]
    [SwaggerOperation(Summary = "Get transactions by subscription id")]
    public async Task<IActionResult> GetTransactionsBySubscriptionId(string subscriptionId)
    {
        var subscriptionGuid = new Guid(subscriptionId);
        var transactions = await _mediator.Send(new GetTransactionForSubscriptionUseCase(subscriptionGuid));
        return Ok(transactions);
    }

    [HttpGet("/bill/{billId}")]
    [SwaggerOperation(Summary = "Get transactions by bill id")]
    public async Task<IActionResult> GetTransactionsByBillId(string billId)
    {
        var billGuid = new Guid(billId);
        var transactions = await _mediator.Send(new GetTransactionsByBillIdUseCase(billGuid));
        return Ok(transactions);
    }
    
    

}