using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterFAX.Api;

namespace SendFaxConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            SendFaxAsync().Wait();
        }

        static private async Task SendFaxAsync()
        {

            var interfax = new FaxClient(username: "ekrauss", password: "V2shC2t1!");
            int returnVal = 0;

            try
            {
                var options = new SendOptions { FaxNumber = "+18187015973" };

                // with a path
                var faxDocument = interfax.Documents.BuildFaxDocument(@".\FaxDocs\fax.txt");
                var messageId = await interfax.Outbound.SendFax(faxDocument, options);
                returnVal = 1;

            }
            catch (Exception ex)
            {
                returnVal = 0;
                Console.WriteLine(ex.Message);
            }

        }


    }
}
