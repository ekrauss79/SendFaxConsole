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
using System.Net.Mail;
using System.Net.Mime;

namespace SendFaxConsole
{
    public class SendMail
    {
        // Create a new Exchange service object

        public static string SendExchangeMail(string clientEmailAddress, string fileLocation, string gmailUserNamne, string gmailPassword, string gmailFromAddress, string attachmentFilename, string clientName, string bodyType)
        {
            try
            {

                string emailBody = "";

                if (bodyType.ToLower() == "monthly")
                {
                    emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody(clientName);
                } else if (bodyType.ToLower() == "weekly")
                {
                    emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody(clientName);
                }
                else
                {
                    emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody(clientName);
                }

                // Command line argument must the the SMTP host.
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(gmailUserNamne, gmailPassword);

                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(gmailFromAddress);
                message.From = fromAddress;
                message.Subject = "LA Care Waste & Abuse Peer-to-Peer Report";
                message.Body = emailBody;
                message.To.Add(clientEmailAddress);
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachmentFilename != null)
                {
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentFilename, System.Net.Mime.MediaTypeNames.Application.Octet);
                    System.Net.Mime.ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(attachment);
                }

                client.Send(message);

                return "success";

            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }

        }

    }
}
