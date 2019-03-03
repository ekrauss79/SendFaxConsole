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
using System.Text;
using SendFaxConsole;


//using SendFaxConsole.

/*******************************************************************
 * 1/17/2019 - you need to externalize the usernale the program uses
 * 
 * 1/17/2019 - you should also create a debug flag that will change 
 * it back to the development user
 * 
 * 1/17/2019 - You need to now test with it released on your system
 * and then install it internally on Bens system
 * 
 * *****************************************************************/

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

            //pull in the config file string values
            string myDEVUserName = System.Configuration.ConfigurationSettings.AppSettings["DEV_Username"];
            string myDEVPassword = System.Configuration.ConfigurationSettings.AppSettings["DEV_Password"];
            string myPRODUserName = System.Configuration.ConfigurationSettings.AppSettings["PROD_Username"];
            string myPRODPassword = System.Configuration.ConfigurationSettings.AppSettings["PROD_Password"];
            string myPRODFlag = System.Configuration.ConfigurationSettings.AppSettings["IsPROD"];
            string myGmailUsername = System.Configuration.ConfigurationSettings.AppSettings["GmailUsername"];
            string myGmailPassword = System.Configuration.ConfigurationSettings.AppSettings["GmailPassword"];
            string myGmailFromAddress = System.Configuration.ConfigurationSettings.AppSettings["GmailFromAddress"];
            string myGmailWaitTime = System.Configuration.ConfigurationSettings.AppSettings["GmailWaitTime"];
            string myFaxWaitTime = System.Configuration.ConfigurationSettings.AppSettings["FaxWaitTime"]; ;
            string myCurrentUsername = "";
            string myCurrentPassword = "";
            string myEmailSuccess = "";
            string myRunTYpe = "";
            string myEmailResult = "";


            //create the log file
            StreamWriter myLog;

            if (!File.Exists("C:\\temp\\faxlog.txt"))
            {
                myLog = new StreamWriter("C:\\temp\\faxlog.txt");
            }
            else
            {
                myLog = File.AppendText("C:\\temp\\faxlog.txt");
            }

            //log the event
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("////-------------FAX PROCESS INITIATED------------\\\\"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("The fax software PROD flag is currently set up " + myPRODFlag));
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("The fax software PROD flag is currently set up " + myPRODFlag));

            int totalCount = 0;
            int currentRecordNumber = 1;

            //Create the fax client with the user information
            if (myPRODFlag == "true")
            {
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* SOFTWARE SET TO PRODUCTION MODE *******"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* SOFTWARE SET TO PRODUCTION MODE *******"));
                myCurrentUsername = myPRODUserName;
                myCurrentPassword = myPRODPassword;
            }
            else
            {
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* SOFTWARE SET TO DEVELOPMENT MODE *******"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* SOFTWARE SET TO DEVELOPMENT MODE *******"));
                myCurrentUsername = myDEVUserName;
                myCurrentPassword = myDEVPassword;
            }

            //get the program run type configuration
            ConfigurationModel myConfigModel = new ConfigurationModel();
            myConfigModel = DataProvider.Instance.GetRunTypeConfiguration();

            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* PROGRAM RUN TYPE SET TO " + myConfigModel.ConfigurationValue + "*******"));
            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* PROGRAM RUN TYPE SET TO " + myConfigModel.ConfigurationValue + "*******"));

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
                    if (myConfigModel.ConfigurationValue == "email")
                    {
                        if (!String.IsNullOrEmpty(faxRequest.Client_Email))
                        {
                            //log the event
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing email records " + currentRecordNumber + " of " + totalCount));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing email records " + currentRecordNumber + " of " + totalCount));

                            //send the email
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                            myEmailSuccess = SendFaxConsole.SendMail.SendExchangeMail(faxRequest.Client_Email, faxRequest.Fax_File_Location, myGmailUsername, myGmailPassword, myGmailFromAddress, faxRequest.Fax_File_Location);

                            //log the event
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing email record for address " + faxRequest.Client_Email));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing email record for address " + faxRequest.Client_Email));

                            //increment the record number
                            currentRecordNumber++;

                            if (myEmailSuccess == "success") //Successful email
                            {

                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                                Thread.Sleep(Convert.ToInt32(myGmailWaitTime));

                                //update the record in the model to success
                                faxRequest.Fax_Status = "success";

                                //write to the audit file
                                myEmailResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Attempted to write to the audit file in success process."));

                                if (myEmailResult == "success")
                                {
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Successful write to the audit file."));
                                    //Delete the record from the transaction file
                                    myEmailResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);

                                    if (myEmailResult == "success")
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " successfully sent!"));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " successfully sent!"));
                                    }
                                    else
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailResult));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailResult));
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " successfully sent, but with audit issues.  Please refer to the log for more details."));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + "  successfully sent, but with audit issues.  Please refer to the log for more details."));
                                    }

                                }
                                else
                                {
                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                }

                                Debug.WriteLine("Email Sent!");
                            }
                            else //Failure
                            {
                                //log the event
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " Failed"));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " Failed"));

                                //update the record in the model to success
                                faxRequest.Fax_Status = "failure";

                                //write to the aidut file
                                myEmailResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Attempted to write to the audit file in failure process."));

                                if (myEmailResult == "success")
                                {
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Successful write to the audit file."));
                                    //Delete the record from the transaction file
                                    myEmailResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);

                                    if (myEmailResult == "success")
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " failed to send."));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " failed to send."));
                                    }
                                    else
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                    }

                                }
                                else
                                {
                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                }

                                //log the event
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage($"Error: " + myEmailSuccess));
                                Debug.WriteLine($"Error: " + myEmailSuccess);
                            }

                        }
                        else
                        {
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for  " + faxRequest.Client_Name + ".  No email will be sent."));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for  " + faxRequest.Client_Name + ".  No email will be sent."));
                        }

                    }
                    else //this means that the runtype is either fax or both
                    {

                        //this is the DEV account login
                        var interfax = new FaxClient(username: myCurrentUsername, password: myCurrentPassword);
                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: " + myCurrentUsername));
                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: " + myCurrentUsername));

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

                        if (myConfigModel.ConfigurationValue == "both")
                        {

                            if (!String.IsNullOrEmpty(faxRequest.Client_Email))
                            {
                                //send the email
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                                myEmailSuccess = SendFaxConsole.SendMail.SendExchangeMail(faxRequest.Client_Email, faxRequest.Fax_File_Location, myGmailUsername, myGmailPassword, myGmailFromAddress, faxRequest.Fax_File_Location);


                                if (myEmailSuccess == "success") //Successful email
                                {

                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                                    Thread.Sleep(Convert.ToInt32(myGmailWaitTime));

                                    //update the record in the model to success
                                    faxRequest.Fax_Status = "success";

                                    //write to the audit file
                                    myEmailResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Attempted to write to the audit file in success process."));

                                    if (myEmailResult == "success")
                                    {
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Successful write to the audit file."));

                                        if (myEmailResult == "success")
                                        {
                                            //log the event
                                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " successfully sent!"));
                                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " successfully sent!"));
                                        }
                                        else
                                        {
                                            //log the event
                                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailResult));
                                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailResult));
                                            //log the event
                                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " successfully sent, but with audit issues.  Please refer to the log for more details."));
                                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + "  successfully sent, but with audit issues.  Please refer to the log for more details."));
                                        }

                                    }
                                    else
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                    }

                                    Debug.WriteLine("Email Sent!");
                                }
                                else //Failure
                                {
                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " Failed"));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Email + " Failed"));

                                    //update the record in the model to success
                                    faxRequest.Fax_Status = "failure";

                                    //write to the aidut file
                                    myEmailResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Attempted to write to the audit file in failure process."));

                                    if (myEmailResult == "success")
                                    {
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Successful write to the audit file."));
                                        //Delete the record from the transaction file
                                        myEmailResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);

                                        if (myEmailResult == "success")
                                        {
                                            //log the event
                                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " failed to send."));
                                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " failed to send."));
                                        }
                                        else
                                        {
                                            //log the event
                                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                        }

                                    }
                                    else
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myEmailSuccess));
                                    }

                                    //log the event
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage($"Error: " + myEmailSuccess));
                                    Debug.WriteLine($"Error: " + myEmailSuccess);
                                    break;
                                }

                            }
                            else
                            {
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for  " + faxRequest.Client_Name + ".  No email will be sent."));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for  " + faxRequest.Client_Name + ".  No email will be sent."));
                            }

                        }

                        //log the event
                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax record for number " + faxRequest.Client_Fax_Number));
                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing Fax record for number " + faxRequest.Client_Fax_Number));

                        //increment the record number
                        currentRecordNumber++;

                        string myResult = "";

                        // wait for the fax to be delivered successfully
                        while (true)
                        {
                            // load the fax's status
                            var fax = await interfax.Outbound.GetFaxRecord(faxId);

                            // sleep if pending for 30 seconds
                            if (fax.Status < 0)
                            {
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing..."));
                                Thread.Sleep(Convert.ToInt32(myFaxWaitTime));
                            }
                            else if (fax.Status == 0) //Successful Fax
                            {

                                //update the record in the model to success
                                faxRequest.Fax_Status = "success";

                                //write to the audit file
                                myResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Attempted to write to the audit file in success process."));

                                if (myResult == "success")
                                {
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Successful write to the audit file."));
                                    //Delete the record from the transaction file
                                    myResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);

                                    if (myResult == "success")
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent!"));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent!"));
                                    }
                                    else
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " successfully sent, but with audit issues.  Please refer to the log for more details."));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + "  successfully sent, but with audit issues.  Please refer to the log for more details."));
                                    }

                                }
                                else
                                {
                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                }

                                Debug.WriteLine("Fax Sent!");
                                break;
                            }
                            else //Failure
                            {
                                //log the event
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " Failed"));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " Failed"));

                                //update the record in the model to success
                                faxRequest.Fax_Status = "failure";

                                //write to the aidut file
                                myResult = DataProvider.Instance.InsertFaxRequestAuditRecord(faxRequest);
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Attempted to write to the audit file in failure process."));

                                if (myResult == "success")
                                {
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("AUDIT PROCESS:  Successful write to the audit file."));
                                    //Delete the record from the transaction file
                                    myResult = DataProvider.Instance.DeleteFaxRequest(faxRequest);

                                    if (myResult == "success")
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " failed to send."));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Fax for number " + faxRequest.Client_Fax_Number + " failed to send."));
                                    }
                                    else
                                    {
                                        //log the event
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                    }

                                }
                                else
                                {
                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(myResult));
                                }

                                //log the event
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage($"Error: {fax.Status}"));
                                Debug.WriteLine($"Error: {fax.Status}");
                                break;
                            }

                        }

                    }
                }

                //close the stream
                myLog.Close();

            }
            catch (Exception ex)
            {
                //log the event
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(ex.InnerException.ToString()));
                Debug.WriteLine(ex.InnerException);

                //close the stream
                myLog.Close();
            }

        }

    }
}
