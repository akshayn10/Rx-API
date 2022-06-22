using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Primary.Organization;
using Rx.Domain.DTOs.Primary.Organization;
using Swashbuckle.AspNetCore.Annotations;


namespace Rx.API.Controllers.Primary
{
    [Route("api/organization")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly IMediator _mediator;


        public OrganizationController(ILogger<OrganizationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Organizations")]
        public async Task<IActionResult> GetOrganizations()
        {
            _logger.LogInformation("Executing");
            var organizations = await _mediator.Send(new RetrieveOrganizationUseCase());
            return Ok(organizations);

        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Organization by Id")]
        public async Task<IActionResult> GetOrganization(string id)
        {
            var organization = await _mediator.Send(new GetOrganizationByIdUseCase(Guid.Parse(id)));
            return Ok(organization);

        }

        [HttpPost("test")]
        [SwaggerOperation(Summary = "Create a new Organization")]
        public async Task<IActionResult> CreateOrganizationTest([FromBody] OrganizationForCreationDto organizationForCreationDto)
        {
            if (organizationForCreationDto is null)
            {
                return BadRequest("OrganizationForCreationDto is null");
            }

            var createdOrganization =
                await _mediator.Send(new CreateOrganizationTestUseCase(organizationForCreationDto));
            return CreatedAtRoute("CreateOrganization", new {id = createdOrganization.Id}, createdOrganization);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Organization")]
        public async Task<IActionResult> CreateOrganization([FromForm] CreateOrganizationRequestDto createOrganizationRequestDto)
        {
            if (createOrganizationRequestDto is null)
            {
                return BadRequest("CreateOrganizationRequestDto is null");
            }

            var createdOrganizationId =
                await _mediator.Send(new CreateOrganizationUseCase(createOrganizationRequestDto));
            return Ok("Organization created with id: " + createdOrganizationId);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an Organization")]
        public async Task<IActionResult> UpdateOrganization(string id,
            [FromForm] EditOrganizationRequestDto editOrganizationRequestDto)
        {
            if (editOrganizationRequestDto is null)
            {
                return BadRequest("UpdateOrganizationRequestDto is null");
            }

            var res = await _mediator.Send(new EditOrganizationUseCase(Guid.Parse(id), editOrganizationRequestDto));
            return Ok("Organization updated with id: " + id);
        }
    }
}
     