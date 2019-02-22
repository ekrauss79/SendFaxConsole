using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFaxConsole.Data.Models
{
    public class FaxRecipientMasterModel
    {
        #region [Properties]

        public int ClientID { get; set; }
        public string Client_Name { get; set; }
        public string Client_Contact_Name { get; set; }
        public string Client_Fax_Number { get; set; }
        public string Client_Phone_Number { get; set; }
        public System.DateTime Audit_Date { get; set; }
        public System.DateTime Audit_Time { get; set; }
        public string Audit_User { get; set; }
        public string Client_Email { get; set; } 

        #endregion
    }
}
