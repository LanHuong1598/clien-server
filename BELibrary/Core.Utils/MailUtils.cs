using BELibrary.Entity;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace BELibrary.Utils
{
    public class MailUtils
    {
        protected static string Generatekey()
        {
            string key = "SHOP";
            for (int i = 0; i < 5; i++)
            {
                String s = CodeUtils.RandomString(4);
                Thread.Sleep(50);
                key += "-" + s;
            }
            return key;
        }

        public static ResponseEmail SendEmail(string mailTo, string subject, string content)
        {
            string code = Generatekey();
            try
            {
                var client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials =
                        new NetworkCredential(
                            "email@gmail.com",
                            "password"),
                    EnableSsl = true,
                };
                var from = new MailAddress("phucnd.hvit@gmail.com", "A - Movie");
                var to = new MailAddress(mailTo);
                var mail = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true,
                };
                client.Send(mail);

                return new ResponseEmail(true, code, "");
                // phải làm cái này ở mail dùng để gửi phải bật lên
                // https://www.google.com/settings/u/1/security/lesssecureapps
            }
            catch (Exception ex)
            {
                return new ResponseEmail(false, code, ex.Message);
            }
        }
    }

    public class ResponseEmail
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Ex { get; set; }

        public ResponseEmail(bool status, string code, string ex)
        {
            this.Status = status;
            this.Code = code;
            this.Ex = ex;
        }
    }
}