using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Transaction;

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
    public async Task<IActionResult> GetTransactions()
    {
        var transactions =await _mediator.Send(new GetTransactionsUseCase());
        return Ok(transactions);
    }
}