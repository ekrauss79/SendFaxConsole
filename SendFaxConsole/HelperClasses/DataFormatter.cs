using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Legacy.WebApp.HelperClasses
{
    public class DataFormatter
    {

        public static String HandleNullInput(string value1, string value2)
        {
            try
            {
                return value1 + " " + value2;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static double RoundDouble(double value, int position)
        {
            double dblValue = 0.0;

            try
            {
                dblValue = Math.Round(value, position);
            }
            catch (Exception ex)
            {

            }

            return dblValue;
        }

        public static String ConvertShortYesNo(String inValue)
        {
            String returnValue;

            if (inValue == "Yes" || inValue == "Y")
            {
                returnValue = "Y";
            }
            else
            {
                returnValue = "N";
            }

            return returnValue;
        }


        public static String ConvertYesNo(String inValue)
        {
            String returnValue;

            if (inValue == "Y")
            {
                returnValue = "Yes";
            }
            else
            {
                returnValue = "No";
            }

            return returnValue;
        }

        public static String ConvertDateTimeToShortDateString(DateTime? inDate)
        {
            String outDate = "";

            if (inDate.HasValue)
            {
                outDate = String.Format("{0:MM/dd/yyyy}", inDate);
            }

            return outDate;
        }

        public static String ConvertDateTimeToShortDateString(DateTime inDate)
        {
            String outDate = "";

            outDate = String.Format("{0:MM/dd/yyyy}", inDate);

            return outDate;
        }

        public static double ConvertByteToMegaByte(long bytes)
        {

            double megaBytes = 0.0;

            try
            {
                megaBytes = ((bytes / 1024)/1024);
            }
            catch (Exception e)
            {
                megaBytes = 0.0;
            }

            return megaBytes;
        }

        public static double ConvertMinuteToMS(int minutes)
        {

            double milliseconds = 0.0;

            try
            {
                milliseconds = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            }
            catch (Exception e)
            {
                milliseconds = 0.0;
            }

            return milliseconds;
        }

        public static double ConvertMSToMinute(long milliseconds)
        {

            double minutes = 0.0;

            try
            {
                minutes = RoundDouble(TimeSpan.FromMilliseconds(milliseconds).TotalMinutes, 1);
            }
            catch (Exception e)
            {
                minutes = 0.0;
            }

            return minutes;
        }
    }
}