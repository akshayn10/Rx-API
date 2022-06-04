using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Rx.API.Controllers.Payment;

[ApiController]
[Route("api/stripe")]
public class StripeController:Controller
{
    private readonly ILogger<StripeController> _logger;
    private readonly string endpointSecret = "whsec_f694f6ea55a5554307dd5e5ffd5421eb413fa838edbfcb96edeb1b2a0a0ae19d";

    public StripeController(ILogger<StripeController> logger)
    {
        _logger = logger;
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
            
            // Handle the event
            _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
            _logger.LogInformation("Unhandled event data: {0}", stripeEvent.Data.Object);
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
    
}