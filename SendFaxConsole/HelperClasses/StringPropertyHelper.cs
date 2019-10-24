using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace SendFaxConsole.HelperClasses
{

    public class StringPropertyHelper
    {

        public static String GetFormattedDecimal(Decimal myInputValue)
        {
            if (myInputValue == null)
                return "0.00";

            String myReturnValue = "0.00";

            try
            {
                myReturnValue = myInputValue.ToString("0.##");
            }
            catch
            {
                   
            }

            return myReturnValue;

        }

        public static string GetFormattedUSDateString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            CultureInfo culture;

            culture = culture = CultureInfo.CreateSpecificCulture("en-CA");

            //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            DateTimeStyles styles = DateTimeStyles.AssumeLocal;

            DateTime dt;
            var r = DateTime.TryParse(str, culture, styles, out dt);

            if (!r)
                return string.Empty;
            else
            {
                return string.Format("{0:MM/dd/yyyy}", dt);
            }
        }

        public static string GetFormattedCADateString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            //CultureInfo culture;

            //culture = culture = CultureInfo.CreateSpecificCulture("en-CA");

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            DateTimeStyles styles = DateTimeStyles.AssumeLocal;

            DateTime dt;
            var r = DateTime.TryParse(str, culture, styles, out dt);

            if (!r)
                return string.Empty;
            else
            {
                return string.Format("{0:dd/MM/yyyy}", dt);
            }
        }


        public static string RemovePhoneMask(string phoneAsString)
        {
            if (string.IsNullOrEmpty(phoneAsString))
                return string.Empty;

            return new string(phoneAsString.Where(c => char.IsDigit(c)).ToArray());
        }

        public static string GetPhoneNumberFormatted(string phoneAsString)
        {
            if (!string.IsNullOrEmpty(phoneAsString))
            {
                phoneAsString = new string(phoneAsString.Where(c => char.IsDigit(c)).ToArray());
                return String.Format("{0:(###) ###-####}", Convert.ToInt64(phoneAsString));
            }
            else
                return string.Empty;
        }

        public static String SetUpper(String inString)
        {

            String outString = "";

            if (!String.IsNullOrEmpty(inString))
            {
                outString = inString.ToUpper();
            }

            return outString;
        }
        //this is the old original message...depricated on 10/23/2019
        public static String getMonthlyEmailBody_old(string clientName)
        {

            String outString = "";

            outString = " Dear " + clientName + "  \n \n \n " + 
                         "RE: NEW WASTE AND ABUSE PREVENTION PROGRAM  \n \n \n " +
                         "L.A. Care Health Plan (L.A. Care) is initiating a waste and abuse prevention program.  The \n" +
                         "program will commence on January 1, 2019.  Currently, Navitus Health Solutions, the PBM for \n" +
                         "L.A. Care, screens claims at point of sale.  This new program adds a further prevention action \n" +
                         "based on historical and current claims to identify potential waste and abuse actions. \n \n" +
                         "This program was developed to identify provider pharmacies that are both variant and \n" +
                         "statistically inconsistent with industry practices. The program does not identify provider \n" +
                         "pharmacies who perform appropriate actions. \n \n" +
                         "Pharmacies will receive monthly reports identifying variance from similar practices for \n" +
                         "geography, pharmacy channel, patient severity, and medication category types.  These reports \n " +
                         "will also provide recommendations for change that can be implemented to reduce the potential \n " +
                         "or waste and abuse.  In addition, quarterly reports will compare results to local and industry \n " +
                         "averages with recommendations for potential changes. \n \n " +
                         "We appreciate your consideration and assistance in helping minimize any waste and abuse \n " +
                         "concerns.  This program should be helpful in identifying and operationalizing changes that \n " +
                         "enhance your practice and help you to confirm good prescribing and dispensing practices.  \n \n " +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and \n " +
                         "by fax at213-438-5776. \n \n " +
                         "Thank you for your time.  We look forward to communicating with you. \n \n " +
                         "Sincerely, \n \n " +
                         "Pharmacy & Formulary Department  \n " +
                         "L.A. Care Health Plan"; 

            return outString;
        }

        public static String getMonthlyEmailBody(string clientName)
        {

            String outString = "";

            outString = " Dear " + clientName + "  \n \n \n " +
                         "RE: NEW WASTE AND ABUSE PREVENTION PROGRAM  \n \n \n " +
                         "As a Health Plan LA Care is charged with exhaustive analyses and changes to Waste & Abuse. \n" +
                         "The Recommendations in this report are crucial for you to adopt and become part of your \n" +
                         "normal procedures.  Waste & Abuse take away from quality and optimal clinical results.  \n " +
                         "Your peers have the same issues as you do, but they have been able to see improvement.  We look \n " +
                         "forward to your improvements in the next few months. \n \n " +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and \n " +
                         "by fax at213-438-5776. \n \n " +
                         "Thank you for your time.  We look forward to communicating with you. \n \n " +
                         "Sincerely, \n \n " +
                         "Pharmacy & Formulary Department  \n " +
                         "L.A. Care Health Plan";

            return outString;
        }



        public static string GenerateRandomString(int length, String allowedChars)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new System.Text.StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}
