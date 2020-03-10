using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFaxConsole.Data.Models
{
    public class FaxRequestQueryModel
    {

        #region [Properties]

        public int FaxRequestID { get; set; }
        public int ClientID { get; set; }
        public string Client_Name { get; set; }
        public string Client_Fax_Number { get; set; }
        public string Fax_File_Location { get; set; }
        public string Fax_Status { get; set; }
        public System.DateTime Date_Requested { get; set; }
        public string Client_Email { get; set; }
        public string Message_Short_Name { get; set;  }
        public int MessageID { get; set; }
        public string Message_Section1_Subject { get; set; }
        public string Message_Section2_Body1 { get; set; }
        public string Message_Section3_Body2 { get; set; }
        public string Message_Section4_Body3 { get; set; }
        public string Message_Section5_Body4 { get; set; }
        public string Message_Section6_Body5 { get; set; }
        public string Message_Section7_Body6 { get; set; }
        public string Message_Section8_Body7 { get; set; }
        public string Message_Section9_Body8 { get; set; }

        #endregion

    }
}
