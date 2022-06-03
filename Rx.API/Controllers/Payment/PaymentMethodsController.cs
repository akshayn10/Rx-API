using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Domain.DTOs.Payment;
using Rx.Domain.Interfaces.Payment;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Rx.API.Controllers.Payment;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/payment")]
public class PaymentMethodsController:Controller
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly IPaymentService _paymentService;

    public PaymentMethodsController(IMediator mediator,IConfiguration configuration,IPaymentService paymentService)
    {
        _mediator = mediator;
        _configuration = configuration;
        _paymentService = paymentService;
    }
    [HttpPost]
    public ActionResult Redirect()
    {
        return Redirect("https://www.google.com");
    }
    public async Task<IActionResult> Index(string customerEmail)
    {
        var customer = await _paymentService.GetCustomerByEmail(customerEmail, PaymentModel.PaymentModelInclude.PaymentMethods);
        if (customer == null)
            return View();
        ViewData["StripePublicKey"] = _configuration.GetSection("Stripe").GetValue<string>("publicKey");
        ViewData["ClientSecret"] = (await _paymentService.PrepareForFuturePayment(customer.Id)).IntentSecret;
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string customerEmail, string paymentMethodId)
    {
        await _paymentService.DeletePaymentMethod(paymentMethodId);
        return RedirectToAction("Index", new { customerEmail = customerEmail });
    }
}