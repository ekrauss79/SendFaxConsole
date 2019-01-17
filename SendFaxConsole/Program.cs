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

            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("****************************************"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*                                      *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("* Welcome to the Fax Automation System *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*                                      *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*   Refer to C:\\Temp\\FaxLog.txt for  *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*           log information            *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*                                      *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("****************************************"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(""));
            SendFaxAsync2().Wait();
        }

        static private async Task SendFaxAsync2()
        {

            //create the log file
            StreamWriter myLog;

            if (!File.Exists("logfile.txt"))
            {
                myLog = new StreamWriter("logfile.txt");
            }
            else
            {
                myLog = File.AppendText("logfile.txt");
            }

            int totalCount = 0;
            int currentRecordNumber = 1;
            var interfax = new FaxClient(username: "erickrauss", password: "V2shC2t1!");
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("////-------------FAX PROCESS INITIATED------------\\\\"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: erickrauss"));
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: erickrauss"));

            //try
            //{

                //get the data
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Requesting dataset for Fax Requests"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Requesting dataset for Fax Requests"));

                List<FaxRequestQueryModel> myModel = new List<FaxRequestQueryModel>();
                myModel = DataProvider.Instance.GetFaxRequest();

                //get the total count for use in the loop
                totalCount = myModel.Count();
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Dataset retrieved.  There are " + totalCount + " record(s)."));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Dataset retrieved.  There are " + totalCount + " record(s)."));

            //loop through the the entire resultset
                foreach (var faxRequest in myModel)
                {

                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax records " + currentRecordNumber + " of " + totalCount));
                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax records " + currentRecordNumber + " of " + totalCount));

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


                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax record for number " + faxRequest.Client_Fax_Number));
                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax record for number " + faxRequest.Client_Fax_Number));

                    currentRecordNumber++;

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
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                            Thread.Sleep(30000);
                        }
                        else if (fax.Status == 0)
                        {
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent!"));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent!"));
                            Debug.WriteLine("Sent!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " Failed"));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " Failed"));
                            Debug.WriteLine($"Error: {fax.Status}");
                            break;
                        }

                    }
                }

            myLog.Close();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.InnerException);
            //}

        }

    }
}
