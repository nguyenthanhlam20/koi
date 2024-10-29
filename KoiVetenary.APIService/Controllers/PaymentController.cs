using KoiVetenary.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace KoiVetenary.APIService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{

    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService context)
    {
        _paymentService = context;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentAsync(int id)
    {
        if (id == 0) return NotFound();
        var payment = await _paymentService.GetPaymentByIdAsync(id);

        if(payment == null) return NotFound();
        return Ok(payment);
    }


    [HttpGet("search")]
    public async Task<IActionResult> GetPaymentAsync([FromQuery] int ownerId, string? searchTerm = "")
    {
        if (ownerId == 0) return NotFound();
        var payments = await _paymentService.SearchPaymentAsync(ownerId, searchTerm!);

        if (payments == null) return NotFound();
        return Ok(payments);
    }

}
