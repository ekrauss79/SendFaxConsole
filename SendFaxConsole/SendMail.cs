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

        public static string SendExchangeMail()
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

            // Set user login credentials

            service.Credentials = new WebCredentials("ekrauss@propharmaconsultants.com", "ProPharma#1");

            try
            {
                //Set Office 365 Exchange Webserivce Url
                string serviceUrl = "https://outlook.office365.com/ews/exchange.asmx";
                service.Url = new Uri(serviceUrl);
                EmailMessage emailMessage = new EmailMessage(service);
                emailMessage.From = "ekrauss@propharmaconsultants.com";
                emailMessage.Subject = "Subject";
                emailMessage.Body = new MessageBody("Cupofdev Exchange Web Service API");
                emailMessage.ToRecipients.Add("ekrauss@gmail.com");
                emailMessage.SendAndSaveCopy();
            }
            catch (AutodiscoverRemoteException exception)
            {
                // handle exception
                throw exception;
            }

            return "true";

        }

    }
}
