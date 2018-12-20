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
