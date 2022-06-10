﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rx.Application.UseCases.Primary.Organization;
using Rx.Domain.DTOs.Email;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Services.Primary;
using Swashbuckle.AspNetCore.Annotations;


namespace Rx.API.Controllers.Primary
{
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;


        public OrganizationController(ILogger<OrganizationController> logger,IMediator mediator,IEmailService emailService)
        {
            _logger = logger;
            _mediator = mediator;
            _emailService = emailService;
        }
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Organizations")]
        public async Task<IActionResult> GetOrganizations()
        {
            _logger.LogInformation("Executing");
            var organizations = await _mediator.Send(new RetrieveOrganizationUseCase());
            return Ok(organizations);


        }

        [HttpPost(Name = "CreateOrganization")]
        [SwaggerOperation(Summary = "Create a new Organization")]
        public async Task<IActionResult> CreateOrganization([FromBody] OrganizationForCreationDto organizationForCreationDto)
        {
            if (organizationForCreationDto is null)
            {
                return BadRequest("OrganizationForCreationDto is null");
            }

            var createdOrganization =await _mediator.Send(new CreateOrganizationUseCase(organizationForCreationDto));
            return CreatedAtRoute("CreateOrganization", new { id = createdOrganization.Id }, createdOrganization);
        }

        [HttpPost("mail")]
        public async Task<IActionResult> SendMail([FromBody] EmailRequest emailRequest)
        {
            await _emailService.SendAsync(emailRequest);
            return Ok();
        }
    }
}       
     