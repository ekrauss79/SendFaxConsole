using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendFaxConsole.DataProvider
{

    public class DataProvider : object
    {
        #region [ Constructors ]

        protected DataProvider()
        {
            _dataContext = new PPPC_AutomatedFaxEntities1();
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
        private PPPC_AutomatedFaxEntities1 _dataContext = null;

        /// <summary>
        /// Gets the value for the get only property named DataContext.	
        ///  
        /// </summary>
        public PPPC_AutomatedFaxEntities1 DataContext
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

        /*
        public int? LoadQuickRegistrationUserInfo(QuickRegistrationModel model, Guid coreUserID)
        {

            try
            {
                ContactModel contactSearchModel;
                int? contactID = 0;

                contactSearchModel = FetchUserSearch(model.PrimaryEmailAddress);

                if (contactSearchModel == null)
                {

                    Contact contactModel = new Contact();

                    contactModel.FirstName = model.FirstName;
                    contactModel.LastName = model.LastName;
                    contactModel.PrimaryEmailAddress = model.PrimaryEmailAddress;
                    contactModel.DateCreated = DateTime.Now;
                    contactModel.IsDiscoverable = "Y";

                    DataContext.Contacts.AddObject(contactModel);
                    DataContext.SaveChanges();

                    contactID = FetchContactID(model.PrimaryEmailAddress);

                    //load user
                    User userModel = new User();

                    userModel.CoreUserID = coreUserID;
                    userModel.ContactID = contactID.Value;

                    DataContext.Users.AddObject(userModel);
                    DataContext.SaveChanges();

                    //if the new registrant was invited, add them as contacts of the inviter and the inviter to the new registered user
                    if (model.regkeytoken.HasValue)
                    {
                        ProfileModel profileModel = new ProfileModel();

                        profileModel = GetProfileContactInfo(model.regkeytoken);

                        if (profileModel != null)
                        {
                            RecordLoadedResult result = new RecordLoadedResult();
                            result = AddContactUser(contactID.Value, model.regkeytoken.Value);
                        }

                    }


                }

                return contactID.Value;
            }
            catch (Exception e)
            {
                return 0;
            }
        }



 
        //fixed
        public RecordLoadedResult LoadNewContact(DefineContactModel model, int userID)
        {

            RecordLoadedResult result = new RecordLoadedResult();

            int? contactID;
            //int? recipientID;

            try
            {

                ContactModel contactSearchModel;

                contactSearchModel = FetchUserSearch(model.EmailAddress);

                if (contactSearchModel == null)
                {
                    Contact contactModel = new Contact();

                    contactModel.FirstName = model.FirstName;
                    contactModel.MiddleName = model.MiddleName;
                    contactModel.LastName = model.LastName;
                    if (!String.IsNullOrEmpty(model.Birthdate))
                    {
                        contactModel.DateOfBirth = DateTime.Parse(model.Birthdate);
                    }
                    contactModel.Address = model.Address;
                    contactModel.City = model.City;
                    contactModel.State = model.State;
                    contactModel.PostalCode = model.ZipCode;
                    contactModel.MobilePhone = model.MobilePhone;
                    contactModel.HomePhone = model.HomePhone;
                    contactModel.PrimaryEmailAddress = model.EmailAddress;
                    contactModel.SecondaryEmailAddress = model.SecondaryEmailAddress;
                    contactModel.DateCreated = DateTime.Now;
                    contactModel.IsDiscoverable = "Y";

                    DataContext.Contacts.AddObject(contactModel);
                    DataContext.SaveChanges();

                }

                contactID = FetchContactID(model.EmailAddress);

                ContactUser myContact = new ContactUser();

                myContact.ContactID = contactID.Value;
                myContact.RelationTypeID = model.RelationTypeID;
                //myContact.Status = (int)RelationshipStatuses.Pending;
                myContact.Status = (int)RelationshipStatuses.Confirmed;
                myContact.UserID = userID;
                myContact.DateCreated = DateTime.Now;

                DataContext.ContactUsers.AddObject(myContact);
                DataContext.SaveChanges();

                result.RecordID = contactID.Value;
                result.ContactID = contactID.Value;
                result.SaveSuccess = true;

            }
            catch (Exception e)
            {
                result.RecordID = 0;
                result.ContactID = 0;
                result.SaveSuccess = false;
                throw e;
            }

            return result;
        }

        */

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

        /*
        //no fix needed
        public Boolean DeleteMessageDelegate(int messageID, int contactID)
        {

            Boolean returnValue = true;

            try
            {
                //MessageDelegate model = new MessageDelegate();

                //model = DataContext.CreateObjectSet<MessageDelegate>().Where(i => i.ContactID == contactID).Where(i => i.MessageID == messageID).FirstOrDefault();

                //if (model != null)
                //{
                //    DataContext.DeleteObject(model);
                //    DataContext.SaveChanges();
                //}
                //else
                //{
                //    returnValue = false;
                //}

                string mySQL = "";

                mySQL = "DELETE FROM dbo.MessageDelegate WHERE MessageID = " + messageID + " AND ContactID = " + contactID;
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

        #region [ GET Method Definitions ]

        #region [ DAO Abstraction Method - Modified: Need Testing ]

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
