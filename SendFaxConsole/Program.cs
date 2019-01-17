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

            //Create the fax client with the user information
            var interfax = new FaxClient(username: "erickrauss", password: "V2shC2t1!");

            //log the event
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("////-------------FAX PROCESS INITIATED------------\\\\"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: erickrauss"));
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: erickrauss"));

            try
            {

                //log the event
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Requesting dataset for Fax Requests"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Requesting dataset for Fax Requests"));

                //get the data
                List<FaxRequestQueryModel> myModel = new List<FaxRequestQueryModel>();
                myModel = DataProvider.Instance.GetFaxRequest();

                //get the total count for use in the loop
                totalCount = myModel.Count();

                //log the event
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Dataset retrieved.  There are " + totalCount + " record(s)."));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Dataset retrieved.  There are " + totalCount + " record(s)."));

                //loop through the the entire resultset
                foreach (var faxRequest in myModel)
                {

                    //log the event
                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax records " + currentRecordNumber + " of " + totalCount));
                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax records " + currentRecordNumber + " of " + totalCount));

                    //Send the fax with options
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

                    string myResult = "";
                
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
                        else if (fax.Status == 0) //Successful Fax
                        {

                            //log the stage
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent!"));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent!"));

                            //update the record in the model to success
                            faxRequest.Fax_Status = "success";

                            //write to the aidut file
                            myResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);

                            if (myResult == "success")
                            {
                                //Delete the record from the transaction file
                                myResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);
                            }
                            else
                            {
                                //log the stage
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                            }

                            Debug.WriteLine("Sent!");
                            break;
                        }
                        else //Failure
                        {
                            //log the stage
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " Failed"));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " Failed"));

                            //update the record in the model to success
                            faxRequest.Fax_Status = "failure";

                            //write to the aidut file
                            myResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);

                            if (myResult == "success")
                            {
                                //Delete the record from the transaction file
                                myResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);
                            }
                            else
                            {
                                //log the stage
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                            }

                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage($"Error: {fax.Status}"));
                            Debug.WriteLine($"Error: {fax.Status}");
                            break;
                        }

                    }
                }

                myLog.Close();

            }
            catch (Exception ex)
            {
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(ex.InnerException.ToString()));
                Debug.WriteLine(ex.InnerException);
                myLog.Close();
            }

        }

    }
}
