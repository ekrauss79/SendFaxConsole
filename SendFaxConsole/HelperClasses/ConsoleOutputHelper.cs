using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFaxConsole.HelperClasses
{
    public class ConsoleOutputHelper
    {
        public static string OutputConsoleMessage(String message)
        {
            string returnVal = "";

            returnVal = DateTime.Now + " ------ " + message;

            return returnVal;
        }
    }
}
