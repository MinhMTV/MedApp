using System;
using System.Diagnostics;

namespace App1.Models
{
    public class GlobalVariables
    {

        public static string CurrentLoggedInUser { get; set; }
        public static int CurrentLoggedInUserID { get; set; }

        public static Stopwatch Stopwatch = new Stopwatch();
        public static TimeSpan TimeSpend { get; set; }
        public static int totalPics { get; set; }
        public static int PicturesRight { get; set; }
        public static int PicturesWrong { get; set; }

        public static bool isNavigation { get; set; } = false;

        public static bool isGallery { get; set; } = false;

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

        public static bool isTimer { get; set; } = false; //Session has Timer adjust by Admin  default: no Timer
        public static int defaultMin { get; set; } = 0; //Session default min are like infinity can be adjust by admin

        public static int defaultSec { get; set; } = 0;//Session default sec are 0

        public static bool isPicAmount { get; set; } = false; //Session has max Number of Pics swiped adjust by Admin  default: no restriction of max pics

        public static int defaultPicCount { get; set; } = 55; // Session default Number of Pics for one Training
    }
}
