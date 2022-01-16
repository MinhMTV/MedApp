using System;
using System.Diagnostics;

namespace App1.Models
{
    public class GlobalVariables
    {

        public static string CurrentLoggedInUser { get; set; }
        public static int CurrentLoggedInUserID { get; set; }

        public static bool isAdmin { get; set; }

        public static Stopwatch Stopwatch = new Stopwatch();
        public static TimeSpan TimeSpend { get; set; }
        public static int totalPics { get; set; }
        public static int PicturesRight { get; set; }
        public static int PicturesWrong { get; set; }

        public static int NroOfAvailablePics
        {
            get { return 55; }
        }
        public static int NrOfAvailableGoodPics
        {
            get { return 33; }
        }
        public static int NrOfAvailableBadPics
        {
            get { return 22; }
        }
        public static string ServerName
        {
            get { return "https://ti.uni-due.de/Rest_API/api/"; }
        }

        public static string ServerIP
        {
            get
            {
                return "ti.uni-due.de";
            }
        }
    }
}
