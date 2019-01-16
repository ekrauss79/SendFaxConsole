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

//using SendFaxConsole.

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

            try
            {

                //get the data
                List<FaxRequestQueryModel> myModel = new List<FaxRequestQueryModel>();
                myModel = DataProvider.Instance.GetFaxRequest();

                //loop through the the entire resultset
                foreach (var faxRequest in myModel)
                {

                     var faxId = await interfax.Outbound.SendFax(
                                interfax.Documents.BuildFaxDocument(faxRequest.Fax_File_Location),
                                new SendOptions
                                {
                                    FaxNumber = faxRequest.Client_Fax_Number,
                                    ShouldScale = true,
                                    PageOrientation = PageOrientation.Landscape, 
                                    PageSize = PageSize.Letter
                                }
                    );

                    /***************************************
                     *  12/19/2018 Nightly notes
                     * When you get back you need to add the logic that will update the 
                     * fax request table with the date sent and the status of the attempt.
                     * Once we do this we will be able to reprocess the faxes that dont send.
                     * 
                     * 
                     * *************************************/
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

        }

    }
}
