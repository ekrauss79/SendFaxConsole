using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SendFaxConsole.Data.Models;
using SendFaxConsole.HelperClasses;

namespace SendFaxConsole.Data
{

    public class DataProvider : object
    {
        #region [ Constructors ]

        protected DataProvider()
        {
            _dataContext = new PPPC_AutomatedFaxEntities2();
        }

        #endregion

        #region [ Data Members and Properties ]

        #region [ Instance Create on First Use Property ]

        /// <summary>
        /// Internal member that stores the value for the Create on First Use 
        /// property named Instance.	
        /// Gets a reference to the singleton for the current DataProvider.
        /// </summary>
        private static DataProvider _instance = null;

        /// <summary>
        /// Gets the value for the  Create on First Use property named 
        /// Instance.	
        /// Gets a reference to the singleton for the current DataProvider.
        /// </summary>
        public static DataProvider Instance
        {
            get
            {
                //-- Check to see if the property is null.  If it is, attempt
                //-- to obtain a value for the internal member.
                if (_instance == null)
                {
                    _instance = new DataProvider();
                }

                return _instance;
            }
        }

        #endregion

        #region [ DataContext Get Only Property ]

        /// <summary>
        /// Internal member that stores the value for the get only property 
        /// named DataContext.	
        ///  
        /// </summary>
        private PPPC_AutomatedFaxEntities2 _dataContext = null;

        /// <summary>
        /// Gets the value for the get only property named DataContext.	
        ///  
        /// </summary>
        public PPPC_AutomatedFaxEntities2 DataContext
        {
            get
            {
                return _dataContext;
            }
        }

        #endregion


        #endregion

        #region [ Calculated Properties ]

        #endregion

        #region [ CRUD Method Definitions - Modified: Need Testing ]

        #region [ INSERT ]


        public string InsertFaxRequestAuditRecord(FaxRequestQueryModel model)
        {
            string returnVal = "failure";
             
            tblFaxRequestMaster_AUDIT myFaxRequestMaster_AUDIT = new tblFaxRequestMaster_AUDIT();

            myFaxRequestMaster_AUDIT.FaxRequestID = model.FaxRequestID;
            myFaxRequestMaster_AUDIT.ClientID = model.ClientID;
            myFaxRequestMaster_AUDIT.Fax_File_Location = model.Fax_File_Location;
            myFaxRequestMaster_AUDIT.Date_Requested = model.Date_Requested;
            myFaxRequestMaster_AUDIT.Date_Last_Sent = DateTime.Now;
            myFaxRequestMaster_AUDIT.Fax_Status = model.Fax_Status;

            try
            {

                DataContext.tblFaxRequestMaster_AUDIT.Add(myFaxRequestMaster_AUDIT);
                DataContext.SaveChanges();
                returnVal = "success";
            }
            catch (Exception ex)
            {
                returnVal = ex.InnerException.ToString();
            }

            return returnVal;
        }
 
        #endregion

        #region [ UPDATE ]

        /*
        public Boolean UpdateMemberImageInfo(double imageMB, int contactID, string updateType)
        {
            Boolean returnValue = false;

            try
            {

                var videoInfo = (from m in DataContext.Members
                                 where m.ContactID == contactID
                                 select m).SingleOrDefault();

                if (videoInfo != null)
                {

                    if (updateType == "ADD")
                    {
                        videoInfo.TotalFiles = videoInfo.TotalFiles + 1;
                        videoInfo.TotalFileMB = videoInfo.TotalFileMB + imageMB;
                    }

                    if (updateType == "REMOVE")
                    {
                        videoInfo.TotalFiles = videoInfo.TotalFiles - 1;
                        videoInfo.TotalFileMB = videoInfo.TotalFileMB - imageMB;
                    }

                    DataContext.SaveChanges();

                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception e)
            {
                returnValue = false;
                throw e;
            }

            return returnValue;

        }

        


        public Boolean UpdateMembershipStatus(int contactID)
        {
            Boolean returnValue = false;

            try
            {

                string mySQL = "";

                mySQL = "UPDATE dbo.Member SET MembershipStatus = 'EXPIRED' WHERE ContactID = " + contactID;
                DataContext.ExecuteStoreCommand(mySQL);
                DataContext.SaveChanges();

            }
            catch (Exception e)
            {
                returnValue = false;
                throw e;
            }

            return returnValue;
        }
        */


        #endregion

        #region [ DELETE ]
        

        public string DeleteFaxRequest(FaxRequestQueryModel model)
        {
            string returnVal = "failure";

            try
            {

                DataContext.lsp_DeleteFaxRequest(model.FaxRequestID);
                DataContext.SaveChanges();
                returnVal = "success";

            }
            catch (Exception ex)
            {
                returnVal = ex.InnerException.ToString();
            }


            return returnVal;

        }
         
        #endregion

        #region [ GET Method Definitions ]

        #region [ DAO Abstraction Method - Modified: Need Testing ]
        
        public List<FaxRequestQueryModel> GetFaxRequest()
        {

            return (from faxRequester in DataContext.tblFaxRequestMasters
                    join faxRecipients in DataContext.tblFaxRecipientMasters
                       on faxRequester.ClientID equals faxRecipients.ClientID
                    select new FaxRequestQueryModel
                    {
                        ClientID = faxRequester.ClientID,
                        FaxRequestID = faxRequester.FaxRequestID,
                        Client_Name = faxRecipients.Client_Contact_Name,
                        Client_Fax_Number = faxRecipients.Client_Fax_Number,
                        Fax_File_Location = faxRequester.Fax_File_Location,
                        Date_Requested = faxRequester.Date_Requested
                    }).ToList();
        }

        /*
        public FundraisingModel GetFundraisingCampaign(int userID, int fundraisingID)
        {
            return (from fundraising in DataContext.FundraisingConfigurations
                    where (((fundraising.AdminUserID == userID) || (fundraising.OperatorUserID == userID)) && (fundraising.FundraisingID == fundraisingID))
                    select new FundraisingModel
                    {
                        FundraisingID = fundraising.FundraisingID,
                        CampaignName = fundraising.CampaignName,
                        FundraisingCode = fundraising.FundraisingCode,
                        CampaignType = fundraising.CampaignType,
                        AdminUserID = fundraising.AdminUserID,
                        OperatorUserID = fundraising.OperatorUserID,
                        CampaignDescription = fundraising.CampaignDescription,
                        BeneficiaryGroup = fundraising.BeneficiaryGroup,
                        CampaignStartDate = fundraising.CampaignStartDate,
                        CampaignEndDate = fundraising.CampaignEndDate,
                        NumberOfReferrals = fundraising.NumberOfReferrals,
                        Enabled = fundraising.Enabled
                    }).FirstOrDefault();
        }

        public List<FundraisingModel> GetFundraisingCampaignList(int userID)
        {
            return (from fundraising in DataContext.FundraisingConfigurations
                    where ((fundraising.AdminUserID == userID) || (fundraising.OperatorUserID == userID))
                    select new FundraisingModel
                    {
                        FundraisingID = fundraising.FundraisingID,
                        CampaignName = fundraising.CampaignName,
                        FundraisingCode = fundraising.FundraisingCode,
                        CampaignType = fundraising.CampaignType,
                        AdminUserID = fundraising.AdminUserID,
                        OperatorUserID = fundraising.OperatorUserID,
                        CampaignDescription = fundraising.CampaignDescription,
                        BeneficiaryGroup = fundraising.BeneficiaryGroup,
                        CampaignStartDate = fundraising.CampaignStartDate,
                        CampaignEndDate = fundraising.CampaignEndDate,
                        NumberOfReferrals = fundraising.NumberOfReferrals,
                        Enabled = fundraising.Enabled
                    }).OrderByDescending(m => m.CampaignStartDate).ToList();
        }
        */
 

        #endregion

        #region [ Event Definitions ]

        #endregion

        #region [ Event Handlers ]

        public string StringNullHandler(string inValue)
        {
            string outValue = "";

            if (String.IsNullOrEmpty(inValue))
            {
                outValue = "";
            }
            else
            {
                outValue = inValue;
            }

            return outValue;
        }

        #endregion

        #endregion

        #endregion

    }

}
