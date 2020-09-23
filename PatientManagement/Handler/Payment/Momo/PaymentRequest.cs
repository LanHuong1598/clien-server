using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;

namespace PatientManagement.Handler.Payment.Momo
{
    internal class PaymentRequest
    {
        public static string SendPaymentRequest(string endpoint, string postJsonString)
        {
            try
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                var stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                var response = (HttpWebResponse)httpWReq.GetResponse();

                var jsonResponse = "";

                using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonResponse += temp;
                    }
                }

                //todo parse it
                return jsonResponse;
                //return new MomoResponse(mtid, jsonresponse);
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}