using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Service.DTO.VNPAY;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace KoiVetenary.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IVnPayService _vnpayService;
        private readonly IConfiguration _configuration;

        public CheckoutController(ICheckoutService checkoutService, IVnPayService vnpayService, IConfiguration configuration)
        {
            _checkoutService = checkoutService;
            _vnpayService = vnpayService;
            _configuration = configuration;
        }

        [HttpGet("create")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<string> Checkout([FromQuery] int appointmentId)
        {
            var appointment = await _checkoutService.Checkout(appointmentId);
            var vnPayModel = new VnPayRequestModel
            {
                Amount = (double)appointment.TotalCost!,
                CreatedDate = DateTime.Now,
                Description = appointment.Notes,
                FullName = $"{appointment.Owner.FirstName} {appointment.Owner.LastName}",
                OrderId = appointment.AppointmentId
            };
            return _vnpayService.CreatePaymentUrl(HttpContext, vnPayModel);
        }

        [HttpPost("insert-payment")]
        public async Task<IActionResult> InsertPaymentAsync([FromBody] VnPayReturnModel model)
        {
            if (model.Vnp_TransactionStatus != "00") return BadRequest();
            var transaction = new Payment
            {
                AppointmentId = Convert.ToInt32(model.Vnp_OrderInfo),
                PaymentDate = DateTime.ParseExact((string)model.Vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                TotalAmount = model.Vnp_Amount,
                TransactionNo = model.Vnp_TransactionNo,
                ResponseCode = model.Vnp_ResponseCode,
                TransactionStatus = model.Vnp_TransactionStatus,
                CreatedAt = DateTime.Now,
                ResponseId = model.Vnp_TransactionStatus,
                TmnCode = model.Vnp_TmnCode,
                TxnRef = model.Vnp_TxnRef,
                Amount = model.Vnp_Amount,
                OrderInfo = "Pay for your fish",
                Message = "Pay at vetenary koi",
                PayDate = DateTime.Now,
                BankCode = model.Vnp_BankCode,
                TransactionType = model.Vnp_CardType,
                SecureHash = model.Vnp_SecureHash,

            };

            var orderId = Convert.ToInt32(model.Vnp_OrderInfo);
            await _checkoutService.CreatePayment(orderId, transaction);
            return Ok();
        }

       
    }
}
