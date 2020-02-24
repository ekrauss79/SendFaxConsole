using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFaxConsole.Data.Models
{
    public class FaxRequestMasterModel
    {

        #region [Properties]

        public int FaxRequestID { get; set; }
        public int ClientID { get; set; }
        public string Fax_File_Location { get; set; }
        public System.DateTime Date_Requested { get; set; }
        public Nullable<System.DateTime> Date_Last_Sent { get; set; }
        public int MessageID { get; set;  }
        public string Message_Body { get; set; }


        #endregion
    }
}
