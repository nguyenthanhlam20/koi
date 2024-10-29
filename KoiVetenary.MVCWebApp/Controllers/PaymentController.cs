using KoiVetenary.Common;
using KoiVetenary.Service;
using KoiVetenary.Service.DTO.VNPAY;
using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.MVCWebApp.Controllers
{
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService service)
        {
            _vnPayService = service;
        }

        public async Task<IActionResult> Create(int id)
        {
            var url = Const.API_Endpoint + "Checkout/create?appointmentId=" + id;
            using var httpClient = new HttpClient();
            using var checkoutResponse = await httpClient.GetAsync(url);
            if (!checkoutResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(PaymentFail));

            var paymentUrl = await checkoutResponse.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(paymentUrl) ? RedirectToAction(nameof(PaymentFail)) : Redirect(paymentUrl);
        }

        [HttpGet("PaymentFail")]
        public IActionResult PaymentFail() => View();

        [HttpGet("PaymentSuccess")]
        public IActionResult PaymentSuccess() => View();

        [HttpGet("PaymentCallBack")]
        public async Task<IActionResult> PaymentCallBack()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                if (response == null || response.Vnp_ResponseCode != "00")
                {
                    TempData["Message"] = $"Error Payment with VNPay: {response?.Vnp_ResponseCode ?? "No response from VNPay"}";
                    return RedirectToAction(nameof(PaymentFail));
                }

                // Step 1: Create VNPay url through api checkout/create
                // Step 2: Redirect to VNPAY url to input user payment info
                // Setp 3: After payment success return to PaymentCallBack to get VNPay response object model
                // Step 4: Call api checkout/insert-payment to store object model to payment table and update status
                // of appointment to "Paid"

                var url = Const.API_Endpoint + "Checkout/insert-payment";
                using var httpClient = new HttpClient();
                using var checkoutResponse = await httpClient.PostAsJsonAsync(url, response);
                if (!checkoutResponse.IsSuccessStatusCode)
                    throw new Exception();

                TempData["Message"] = "Payment with VNPay successfully!";
                return RedirectToAction(nameof(PaymentSuccess));
            }
            catch (Exception)
            {
                TempData["Message"] = "An error have been occured while processing payment.";
                return RedirectToAction(nameof(PaymentFail));
            }
        }
    }
}
