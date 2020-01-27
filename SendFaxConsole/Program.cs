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
 * 5/1/2019 - Making a change to the way I loop through an RS to pull
 * the latest available record
 * 
 * *****************************************************************/

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
            string myFaxWaitTime = System.Configuration.ConfigurationSettings.AppSettings["FaxWaitTime"]; 
            string myCurrentUsername = "";
            string myCurrentPassword = "";
            string myEmailSuccess = "";
            string myRunTYpe = "";
            string myEmailResult = "";
            string myLogName = "";
            int myRandomInt = 0;

            //create the log file
            StreamWriter myLog;

            Random myRandom = new Random();
            myRandomInt = myRandom.Next(0, 1000000);

            myLogName = "faxlog" + myRandomInt.ToString() + ".txt";

            if (!File.Exists("C:\\temp\\" + myLogName))
            {
                myLog = new StreamWriter("C:\\temp\\" + myLogName);
            }
            else
            {
                myLog = File.AppendText("C:\\temp\\" + myLogName);
            }

            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("**********************************************"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*                                            *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("* Welcome to the Fax/Email Automation System *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*                                            *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*   Refer to C:\\Temp\\" + myLogName));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*           log information                  *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("*               v2.2.0                       *"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("**********************************************"));
            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(""));

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
            ConfigurationModel myConfigRunTypeModel = new ConfigurationModel();
            ConfigurationModel myConfigReportTypeModel = new ConfigurationModel();

            try
            {

                myConfigRunTypeModel = DataProvider.Instance.GetRunTypeConfiguration();

                //get the program run type configuration
                myConfigReportTypeModel = DataProvider.Instance.GetReportTypeConfiguration();

                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* PROGRAM RUN TYPE SET TO " + myConfigRunTypeModel.ConfigurationValue + "*******"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* PROGRAM RUN TYPE SET TO " + myConfigRunTypeModel.ConfigurationValue + "*******"));

                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* PROGRAM REPORT TYPE SET TO " + myConfigReportTypeModel.ConfigurationValue + "*******"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("******* PROGRAM REPORT TYPE SET TO " + myConfigReportTypeModel.ConfigurationValue + "*******"));

            }
            catch (Exception e)
            {
                //log the event
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage(e.InnerException.ToString()));
                Debug.WriteLine(e.InnerException);

                //close the stream
                myLog.Close();

            }

            try
            {

                //this is the DEV account login
                var interfax = new FaxClient(username: myCurrentUsername, password: myCurrentPassword);
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: " + myCurrentUsername));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Initiating new Fax Client for user: " + myCurrentUsername));

                //log the event
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Requesting dataset for Fax Requests"));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Requesting dataset for Fax Requests"));

                //get the number of fax requests
                List<FaxRequestQueryModel> myCounterModel = new List<FaxRequestQueryModel>();
                myCounterModel = DataProvider.Instance.GetNumberOfRequests();

                //get the total count for use in the loop
                totalCount = myCounterModel.Count();

                //log the event
                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Dataset retrieved.  There are " + totalCount + " record(s)."));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Dataset retrieved.  There are " + totalCount + " record(s)."));

                /****************************************************
                 * 
                 * 
                 * 
                 *  This is where the main loop starts
                 * 
                 * 
                 * 
                 * 
                 * **************************************************/


                if (totalCount != 0)
                {
                    //loop through the resultset
                    while (totalCount > 0)
                    {


                        //get the number of fax requests
                        List<FaxRequestQueryModel> myInternalCounterModel = new List<FaxRequestQueryModel>();
                        myInternalCounterModel = DataProvider.Instance.GetNumberOfRequests();

                        //get the total count for use in the loop
                        totalCount = myInternalCounterModel.Count();

                        //get the number of fax requests
                        FaxRequestQueryModel faxRequest = new FaxRequestQueryModel();

                        //if there are no more faxes to send because the other programs picked them up, do not let the program process any further
                        if (totalCount == 0)
                        {
                            faxRequest.FaxRequestID = 0;
                        }
                        else
                        {
                            faxRequest = DataProvider.Instance.GetFaxRequest();
                        }

                        //make sure that there is something in the faxRequest
                        if (faxRequest.FaxRequestID != 0)
                        {
                            //log the event
                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Got the single dataset for FaxRequestID " + faxRequest.FaxRequestID));
                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Got the single dataset for FaxRequestID " + faxRequest.FaxRequestID));

                            if (myConfigRunTypeModel.ConfigurationValue.ToLower() == "email")
                            {
                                if (!String.IsNullOrEmpty(faxRequest.Client_Email))
                                {
                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("There are " + totalCount + " email records left to process"));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("There are " + totalCount + " email records left to process"));

                                    //send the email
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                                    myEmailSuccess = SendFaxConsole.SendMail.SendExchangeMail(faxRequest.Client_Email, faxRequest.Fax_File_Location, myGmailUsername, myGmailPassword, myGmailFromAddress, faxRequest.Fax_File_Location, faxRequest.Client_Name, myConfigReportTypeModel.ConfigurationValue, faxRequest.Message_Short_Name, faxRequest.Message_Body);

                                    //log the event
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing email record for address " + faxRequest.Client_Email));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Processing email record for address " + faxRequest.Client_Email));

                                }
                                else
                                {
                                    myEmailSuccess = "failure";
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for " + faxRequest.Client_Name + ".  No email will be sent."));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for " + faxRequest.Client_Name + ".  No email will be sent."));
                                }

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
                                    Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " Failed"));
                                    myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " Failed"));

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
                                            Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " failed to send."));
                                            myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " failed to send."));
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
                            else //this means that the runtype is either fax or both
                            {

                                //log the event
                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("There are " + totalCount + " fax records left to process"));
                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("There are " + totalCount + " fax records left to process"));

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

                                if (myConfigRunTypeModel.ConfigurationValue.ToLower() == "both")
                                {

                                    if (!String.IsNullOrEmpty(faxRequest.Client_Email))
                                    {
                                        //send the email
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Attempting email record for address " + faxRequest.Client_Email));
                                        myEmailSuccess = SendFaxConsole.SendMail.SendExchangeMail(faxRequest.Client_Email, faxRequest.Fax_File_Location, myGmailUsername, myGmailPassword, myGmailFromAddress, faxRequest.Fax_File_Location, faxRequest.Client_Name, myConfigReportTypeModel.ConfigurationValue, faxRequest.Message_Short_Name, faxRequest.Message_Body);

                                    }
                                    else
                                    {
                                        myEmailSuccess = "failure";
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for " + faxRequest.Client_Name + ".  No email will be sent."));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("No Email found for " + faxRequest.Client_Name + ".  No email will be sent."));
                                    }

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
                                        Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " Failed"));
                                        myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " Failed"));

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
                                                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " failed to send."));
                                                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Email for Address " + faxRequest.Client_Name + " failed to send."));
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
                    }
                }

                Console.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Program Run Complete."));
                myLog.WriteLine(ConsoleOutputHelper.OutputConsoleMessage("Program Run Complete."));

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
