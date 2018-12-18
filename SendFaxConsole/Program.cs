using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InterFAX.Api;

namespace SendFaxConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            SendFaxAsync2().Wait();
            //String val = "C:\text.txt";

            //Console.WriteLine(val);
        }

        static private async Task SendFaxAsync()
        {

            var interfax = new FaxClient(username: "ekrauss", password: "V2shC2t1!");

            try
            {
                var options = new SendOptions { FaxNumber = "+18187015973" };

                // with a path
                //var fileDocument = interfax.Documents.BuildFaxDocument(@".\FaxDocs\fax.txt");
                //var messageId = await interfax.Outbound.SendFax(faxDocument, options);
                //returnVal = 1;

            }
            catch (Exception ex)
            {
                //returnVal = 0;
                Console.WriteLine(ex.Message);
            }

        }

        static private async Task SendFaxAsync2()
        {

            var interfax = new FaxClient(username: "ekrauss", password: "V2shC2t1!");
            String myFilePath = @"C:\Users\ekrau\source\repos\SendFaxConsole\SendFaxConsole\FaxDocs\fax.txt";

            try
            {
                var faxId = await interfax.Outbound.SendFax(
                  interfax.Documents.BuildFaxDocument(myFilePath),
                  new SendOptions
                  {
                      FaxNumber = "+18187015973"
                  }
                );

                // wait for the fax to be
                // delivered successfully
                while (true)
                {
                    // load the fax's status
                    var fax = await interfax.Outbound.GetFaxRecord(faxId);
                    // sleep if pending
                    if (fax.Status < 0)
                    {
                        Thread.Sleep(10000);
                    }
                    else if (fax.Status == 0)
                    {
                        Debug.WriteLine("Sent!");
                        break;
                    }
                    else
                    {
                        Debug.WriteLine($"Error: {fax.Status}");
                        break;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

        }

    }
}
