using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rx.Application.UseCases.Primary.Organization;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;


namespace Rx.API.Controllers.Primary
{
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly IMediator _mediator;


        public OrganizationController(ILogger<OrganizationController> logger,IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrganizations()
        {
            _logger.LogInformation("Executing");
            var organizations = await _mediator.Send(new RetrieveOrganizationUseCase());
            return Ok(organizations);


        }

        [HttpPost(Name = "CreateOrganization")]
        public async Task<IActionResult> CreateOrganization([FromBody] OrganizationForCreationDto organizationForCreationDto)
        {
            if (organizationForCreationDto is null)
            {
                return BadRequest("OrganizationForCreationDto is null");
            }

            var createdOrganization =await _mediator.Send(new CreateOrganizationUseCase(organizationForCreationDto));
            return CreatedAtRoute("CreateOrganization", new { id = createdOrganization.Id }, createdOrganization);
        }
    }
}
