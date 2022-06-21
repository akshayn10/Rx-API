using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Primary.SystemSubscription;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Primary.SystemSubscription;

namespace Rx.API.Controllers.Primary;

[ApiController]
[Route("api/organization/subscription")]
[Authorize(Roles = "Owner")]
public class SystemSubscriptionController:ControllerBase
{
    private readonly IMediator _mediator;

    public SystemSubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] SystemSubscriptionForCreationDto dto)
    {
        var result = await _mediator.Send(new CreateSystemSubscriptionUseCase(dto));
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(string id)
    {
        var result = await _mediator.Send(new CancelSystemSubscriptionUseCase(Guid.Parse(id)));
        return Ok(result);
    }
    
}