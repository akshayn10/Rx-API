﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rx.Application.UseCases.Payment;
using Rx.Application.UseCases.Tenant.Subscription;
using Rx.Application.UseCases.Tenant.Webhook;
using Rx.Domain.DTOs.Payment;
using Stripe;
namespace Rx.API.Controllers.Payment;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/stripe")]
public class StripeController:Controller
{
    private readonly ILogger<StripeController> _logger;
    private readonly IMediator _mediator;
    private readonly string endpointSecret = "whsec_f694f6ea55a5554307dd5e5ffd5421eb413fa838edbfcb96edeb1b2a0a0ae19d";

    public StripeController(ILogger<StripeController> logger,IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    //Retrieve Stripe Webhook
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);
            
            if (stripeEvent.Type == Events.PaymentMethodAttached)
            {
                // Handle the event
                _logger.LogInformation("PaymentMethodAttached Webhook Recieved");
                var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                var customerId=await _mediator.Send(new PaymentMethodAttachedUseCase(paymentMethod!.CustomerId,paymentMethod.Card.Last4,paymentMethod.Id));
                await _mediator.Send(new CreateSubscriptionFromWebhookUseCase(customerId));
            }

            if (stripeEvent.Type==Events.ChargeSucceeded)
            {
                var chargeSucceeded = stripeEvent.Data.Object as Charge;
                _logger.LogInformation(chargeSucceeded!.ToString());
                if (chargeSucceeded.Description == "addOn")
                {
                    await _mediator.Send(new ActivateAddOnUsageAfterPaymentUseCase(chargeSucceeded.CustomerId,chargeSucceeded.Amount));
                }
                
            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}