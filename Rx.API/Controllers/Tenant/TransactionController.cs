using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Transaction;
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

    [HttpGet("{transactionId:guid}")]
    [SwaggerOperation(Summary = "Get transaction by id")]
    public async Task<IActionResult> GetTransaction(Guid transactionId)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdUseCase(transactionId));
        return Ok(transaction);
    }
}