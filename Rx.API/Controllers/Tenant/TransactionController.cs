using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Middleware;
using Rx.Application.UseCases.Tenant.Transaction;
using Rx.Domain.DTOs.Tenant.Transaction;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant;

[ApiController]
[Route("api/transaction")]
[SubscriptionVerification]
[Authorize(Roles = "FinanceUser")]
public class TransactionController:ControllerBase
{

    private readonly IMediator _mediator;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(IMediator mediator, ILogger<TransactionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("dtos")]
    [SwaggerOperation(Summary = "Get all transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions =await _mediator.Send(new GetTransactionsUseCase());
        return Ok(transactions);
    }
    [HttpGet]
    [SwaggerOperation(Summary = "Get all transactions")]
    public async Task<IActionResult> GetTransactionVms()
    {
        var transactions =await _mediator.Send(new GetTransactionVmsUseCase());
        return Ok(transactions);
    }

    [HttpGet("{transactionId}")]
    [SwaggerOperation(Summary = "Get transaction by id")]
    public async Task<IActionResult> GetTransactionById(Guid transactionId)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdUseCase(transactionId));
        return Ok(transaction);
    }
    [HttpPost("{subscriptionId}")]
    [SwaggerOperation(Summary = "Create transaction")]
    public async Task<IActionResult> CreateTransaction(string subscriptionId,[FromBody] TransactionForCreationDto transactionForCreationDto)
    {

        var transaction = await _mediator.Send(new CreateTransactionUseCase(Guid.Parse(subscriptionId),transactionForCreationDto));
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


    
    

}