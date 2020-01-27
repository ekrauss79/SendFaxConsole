using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFaxConsole.Data.Models
{
    public class MessageModel
    {

        #region [Properties]

        public int MessageID { get; set; }
        public string Message_Short_Name { get; set; }
        public string Message_Short_Description { get; set; }
        public string Message_Body { get; set; }

        #endregion
    }
}
