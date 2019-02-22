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

        #endregion

    }
}
