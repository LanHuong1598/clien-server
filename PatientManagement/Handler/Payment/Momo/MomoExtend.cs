using System;
using Newtonsoft.Json.Linq;

namespace PatientManagement.Handler.Payment.Momo
{
    public class MomoExtend
    {
        public static string GenUrlPay(string orderInfo, string returnUrl, string notifyUrl, string amount, string orderId)
        {
            //request params need to request to MoMo system
            const string endPoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";

            const string partnerCode = "MOMO5RGX20191128";

            const string accessKey = "M8brj9K6E22vXoDB";
            const string serectKey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";

            var requestId = Guid.NewGuid().ToString();
            var extraData = "";

            //Before sign HMAC SHA256 signature
            var rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderId + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyUrl + "&extraData=" +
                extraData;

            var crypto = new MoMoSecurity();
            //sign signature SHA256
            var signature = crypto.SignSha256(rawHash, serectKey);

            //build body json request
            var message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyUrl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };
            var responseFromMomo = PaymentRequest.SendPaymentRequest(endPoint, message.ToString());

            var jMessage = JObject.Parse(responseFromMomo);
            return jMessage.GetValue("payUrl").ToString();
        }
    }
}