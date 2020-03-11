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
using SendFaxConsole.Data.Models;

namespace SendFaxConsole
{
    public class SendMail
    {
        // Create a new Exchange service object
        public static string SendExchangeMail(FaxRequestQueryModel myFaxRequestQueryModel, string gmailUserNamne, string gmailPassword, string gmailFromAddress)
        {
            try
            {

                string emailBody = "";

                switch (myFaxRequestQueryModel.Message_Short_Name.ToLower())
                {
                    case "monthly good":
                        
                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_good(myFaxRequestQueryModel.Client_Name);
                        break;
                        
                    case "monthly bad":
                        
                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_bad(myFaxRequestQueryModel.Client_Name);
                        break;

                    case "custom 2 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_2_section(myFaxRequestQueryModel);
                        break;

                    case "custom 3 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_3_section(myFaxRequestQueryModel);
                        break;

                    case "custom 4 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_4_section(myFaxRequestQueryModel);
                        break;

                    case "custom 5 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_5_section(myFaxRequestQueryModel);
                        break;

                    case "custom 6 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_6_section(myFaxRequestQueryModel);
                        break;

                    case "custom 7 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_7_section(myFaxRequestQueryModel);
                        break;

                    case "custom 8 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_8_section(myFaxRequestQueryModel);
                        break;

                    case "custom 9 sections":

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_custom_9_section(myFaxRequestQueryModel);
                        break;


                    default:

                        emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_good(myFaxRequestQueryModel.Client_Name);
                        break;
                }

                /** OLD CODE replaced by switch sttement
                if (messageType.ToLower() == "monthly good")
                {
                    emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody_good(clientName);
                } else if (bodyType.ToLower() == "weekly")
                {
                    emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody(clientName);
                }
                else
                {
                    emailBody = HelperClasses.StringPropertyHelper.getMonthlyEmailBody(clientName);
                } **/

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
                message.To.Add(myFaxRequestQueryModel.Client_Email);
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (myFaxRequestQueryModel.Fax_File_Location != null)
                {
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(myFaxRequestQueryModel.Fax_File_Location, System.Net.Mime.MediaTypeNames.Application.Octet);
                    System.Net.Mime.ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(myFaxRequestQueryModel.Fax_File_Location);
                    disposition.ModificationDate = File.GetLastWriteTime(myFaxRequestQueryModel.Fax_File_Location);
                    disposition.ReadDate = File.GetLastAccessTime(myFaxRequestQueryModel.Fax_File_Location);
                    disposition.FileName = Path.GetFileName(myFaxRequestQueryModel.Fax_File_Location);
                    disposition.Size = new FileInfo(myFaxRequestQueryModel.Fax_File_Location).Length;
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
