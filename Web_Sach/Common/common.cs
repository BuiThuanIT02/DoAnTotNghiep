using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Xml.Linq;

namespace Web_Sach.Common
{
    public class common
    {
        private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
        private static string Email = ConfigurationManager.AppSettings["Email"];
        public static bool SendMail(string name, string subject, string content, string toMail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; //host name
                    smtp.Port = 587; //port number
                    smtp.EnableSsl = true; //whether your smtp server requires SSL
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    //smtp.Credentials = new NetworkCredential(Email, password);
                    //smtp.Timeout = 20000;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = Email,
                        Password = password
                    };
                }
                MailAddress fromAddress = new MailAddress(Email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body =content;
                smtp.Send(message);
                rs = true;
            }

            catch (Exception)
            {
                rs = false;
            }
            return rs;
           

        }








        public static string HtmlRate(int? rate)
        {
            var str = "";
            if (rate == 1)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 2)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 3)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 4)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 5)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>";
            }
            return str;

        }

    }
}