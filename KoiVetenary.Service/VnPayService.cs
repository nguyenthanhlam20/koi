using KoiVetenary.Service.DTO.VNPAY;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{

    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context, VnPayRequestModel model);
        public VnPayReturnModel PaymentExecute(IQueryCollection collection);
    }
    public class VnPayService : IVnPayService
    {

        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext context, VnPayRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); // Convert to string

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress.ToString()); // Get IP address
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", model.OrderId.ToString());
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);

            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public VnPayReturnModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = long.Parse(vnpay.GetResponseData("vnp_TxnRef")); // Parse to long
            var vnp_TransactionId = long.Parse(vnpay.GetResponseData("vnp_TransactionNo")); // Parse to long
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var vnp_BankCode = vnpay.GetResponseData("vnp_BankCode");
            var vnp_CardType = vnpay.GetResponseData("vnp_CardType");
            var vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_TmnCode = vnpay.GetResponseData("vnp_TmnCode");
            var vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            var vnp_PayDate = vnpay.GetResponseData("vnp_PayDate");
            var vnp_Amount = long.Parse(vnpay.GetResponseData("vnp_Amount"));

            var s = _config["VnPay:HashSecret"];
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPayReturnModel
                {
                    Success = false
                };
            }

            return new VnPayReturnModel
            {
                Success = true,
                Vnp_OrderInfo = vnp_OrderInfo,
                Vnp_BankTranNo = vnp_orderId.ToString(),
                Vnp_TransactionNo = vnp_TransactionId.ToString(),
                Vnp_SecureHash = vnp_SecureHash,
                Vnp_ResponseCode = vnp_ResponseCode,
                Vnp_BankCode = vnp_BankCode,
                Vnp_CardType = vnp_CardType,
                Vnp_TxnRef = vnp_TxnRef,
                Vnp_TmnCode = vnp_TmnCode,
                Vnp_TransactionStatus = vnp_TransactionStatus,
                Vnp_PayDate = vnp_PayDate,
                Vnp_Amount = vnp_Amount
            };
        }
    }
}
