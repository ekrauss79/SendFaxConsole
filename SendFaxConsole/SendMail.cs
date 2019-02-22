using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InterFAX.Api;
using SendFaxConsole.Data;
using SendFaxConsole.HelperClasses;
using SendFaxConsole.Data.Models;
using InterFAX.Api.Dtos;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;


namespace SendFaxConsole
{
    public class SendMail
    {
        // Create a new Exchange service object

        public static string SendExchangeMail(string clientEmailAddress, string fileLocation, string gmailUserNamne, string gmailPassword, string gmailFromAddress)
        {
            try
            {
                // Command line argument must the the SMTP host.
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(gmailUserNamne, gmailPassword);

                MailMessage mm = new MailMessage(gmailFromAddress, clientEmailAddress, "This is the Subject", "This is the body");
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);

                return "true";

            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }

        }

    }
}
