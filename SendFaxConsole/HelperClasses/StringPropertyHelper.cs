using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using SendFaxConsole.Data.Models;

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
        public static String getMonthlyEmailBody_good(string clientName)
        {

            String outString = "";

            outString = " Dear " + clientName + "  <p><p><p>" + 
                         "RE: NEW WASTE AND ABUSE PREVENTION PROGRAM  <p><p><p>" +
                         "L.A. Care Health Plan (L.A. Care) is initiating a waste and abuse prevention program.  The <p>" +
                         "program will commence on January 1, 2019.  Currently, Navitus Health Solutions, the PBM for <p>" +
                         "L.A. Care, screens claims at point of sale.  This new program adds a further prevention action <p>" +
                         "based on historical and current claims to identify potential waste and abuse actions. <p><p>" +
                         "This program was developed to identify provider pharmacies that are both variant and <p>" +
                         "statistically inconsistent with industry practices. The program does not identify provider <p>" +
                         "pharmacies who perform appropriate actions. <p><p>" +
                         "Pharmacies will receive monthly reports identifying variance from similar practices for <p>" +
                         "geography, pharmacy channel, patient severity, and medication category types.  These reports <p>" +
                         "will also provide recommendations for change that can be implemented to reduce the potential <p>" +
                         "or waste and abuse.  In addition, quarterly reports will compare results to local and industry <p>" +
                         "averages with recommendations for potential changes. <p><p>" +
                         "We appreciate your consideration and assistance in helping minimize any waste and abuse <p>" +
                         "concerns.  This program should be helpful in identifying and operationalizing changes that <p>" +
                         "enhance your practice and help you to confirm good prescribing and dispensing practices.  <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan"; 

            return outString;
        }

        public static String getMonthlyEmailBody_bad(string clientName)
        {

            String outString = "";

            outString = " Dear " + clientName + "  <p><p><p>" +
                         "RE: NEW WASTE AND ABUSE PREVENTION PROGRAM  <p><p><p>" +
                         "As a Health Plan LA Care is charged with exhaustive analyses and changes to Waste & Abuse. <p>" +
                         "The Recommendations in this report are crucial for you to adopt and become part of your <p>" +
                         "normal procedures.  Waste & Abuse take away from quality and optimal clinical results.  <p>" +
                         "Your peers have the same issues as you do, but they have been able to see improvement.  We look <p>" +
                         "forward to your improvements in the next few months. <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }

        public static String getMonthlyEmailBody_custom_full(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p> " +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p> " +
                         myFaxRequestQueryModel.Message_Section2_Body1;

            return outString;
        }


        public static String getMonthlyEmailBody_custom_2_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p> " +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p> " +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p> " +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p> " +
                         "by fax at213-438-5776. <p><p> " +
                         "Thank you for your time.  We look forward to communicating with you. <p><p> " +
                         "Sincerely, <p><p> " +
                         "Pharmacy & Formulary Department  <p> " +
                         "L.A. Care Health Plan";

            return outString;
        }

        public static String getMonthlyEmailBody_custom_3_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }

        public static String getMonthlyEmailBody_custom_4_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section4_Body3 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }

        public static String getMonthlyEmailBody_custom_5_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section4_Body3 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section5_Body4 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }

        public static String getMonthlyEmailBody_custom_6_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section4_Body3 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section5_Body4 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section6_Body5 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }


        public static String getMonthlyEmailBody_custom_7_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section4_Body3 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section5_Body4 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section6_Body5 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section7_Body6 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }


        public static String getMonthlyEmailBody_custom_8_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section4_Body3 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section5_Body4 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section6_Body5 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section7_Body6 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section8_Body7 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
                         "L.A. Care Health Plan";

            return outString;
        }


        public static String getMonthlyEmailBody_custom_9_section(FaxRequestQueryModel myFaxRequestQueryModel)
        {

            String outString = "";

            outString = " Dear " + myFaxRequestQueryModel.Client_Name + "  <p><p><p>" +
                         myFaxRequestQueryModel.Message_Section1_Subject + "<p><p><p>" +
                         myFaxRequestQueryModel.Message_Section2_Body1 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section3_Body2 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section4_Body3 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section5_Body4 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section6_Body5 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section7_Body6 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section8_Body7 + " <p><p>" +
                         myFaxRequestQueryModel.Message_Section9_Body8 + " <p><p>" +
                         "If you have any questions about this program, you can reach us by phone at 213-973-0065 and <p>" +
                         "by fax at213-438-5776. <p><p>" +
                         "Thank you for your time.  We look forward to communicating with you. <p><p>" +
                         "Sincerely, <p><p>" +
                         "Pharmacy & Formulary Department  <p>" +
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
