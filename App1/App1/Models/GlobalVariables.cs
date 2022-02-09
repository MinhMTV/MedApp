using System;
using System.Diagnostics;

namespace App1.Models
{
    public class GlobalVariables
    {


        public static Stopwatch Stopwatch = new Stopwatch();
        public static bool isTraining { get; set; } = false; //check if Trainingspage is activ true or not

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
    }
}
