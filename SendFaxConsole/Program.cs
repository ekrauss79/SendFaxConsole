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
        }

        static private async Task SendFaxAsync2()
        {

            var interfax = new FaxClient(username: "erickrauss", password: "V2shC2t1!");
            String myFilePath = @"C:\Users\ekrau\source\repos\SendFaxConsole\SendFaxConsole\FaxDocs\testfax.pdf";

            try
            {
                var faxId = await interfax.Outbound.SendFax(
                  interfax.Documents.BuildFaxDocument(myFilePath),
                  new SendOptions
                  {
                      FaxNumber = "+18187010249"
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
