using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace App1.Models
{
    public class Settings
    {
        public static bool FirstRun
        {
            get => Preferences.Get(nameof(FirstRun), true);
            set => Preferences.Set(nameof(FirstRun), value);
        }

        //----------------------------------------------------Admin Settings -------------------------------------------------
        public static bool isTimer //Session has Timer adjust by Admin  default: no Timer
        {
            get => Preferences.Get(nameof(isTimer), false); 
            set => Preferences.Set(nameof(isTimer), value);
        } 
        public static bool isPicAmount //Session has max Number of Pics swiped adjust by Admin  default: no restriction of max pics
        {
            get => Preferences.Get(nameof(isPicAmount), false);
            set => Preferences.Set(nameof(isPicAmount), value);
        }

        public static int defaultMin //Session default min are like infinity can be adjust by admin
        {
            get => Preferences.Get(nameof(defaultMin), 0);
            set => Preferences.Set(nameof(defaultMin), value);
        }
        public static int defaultSec //Session default sec are 0
        {
            get => Preferences.Get(nameof(defaultSec), 0);
            set => Preferences.Set(nameof(defaultSec), value);
        }

        public static int defaultPicCount  // Session default Number of Pics for one Training
        {
            get => Preferences.Get(nameof(defaultPicCount), 55); //embedded Pics Value
            set => Preferences.Set(nameof(defaultPicCount), value);
        }

        public static int defaultCupCount //Default number of trainings to get a new Cup {
        {
            get => Preferences.Get(nameof(defaultCupCount), 5); //embedded Pics Value
            set => Preferences.Set(nameof(defaultCupCount), value);
        }

        public static bool isAscending  //set default sort by ascending
        {
            get => Preferences.Get(nameof(isAscending), true);
            set => Preferences.Set(nameof(isAscending), value);
        }

        public static bool isImageAscending  //set default sort for images by ascending 
        {
            get => Preferences.Get(nameof(isImageAscending), true);
            set => Preferences.Set(nameof(isImageAscending), value);
        }

        public static bool isGoodImageAscending  //set default sort for good images by ascending 
        {
            get => Preferences.Get(nameof(isGoodImageAscending), true);
            set => Preferences.Set(nameof(isGoodImageAscending), value);
        }

        public static bool isBadImageAscending  //set default sort for bad images by ascending 
        {
            get => Preferences.Get(nameof(isBadImageAscending), true);
            set => Preferences.Set(nameof(isBadImageAscending), value);
        }

        public static string OrderBy  //set order of usercollection, default is empty string, which leads to userdbid
        {
            get => Preferences.Get(nameof(OrderBy), "");
            set => Preferences.Set(nameof(OrderBy), value);
        }

        //----------------------------------------------------User Settings -------------------------------------------------



        //----------------------------------------------------General Settings -------------------------------------------------
        public static string loginUser  //check in preferences if a user was loggin, if not default value is false
        {
            get => Preferences.Get(nameof(loginUser),"false"); //default no User is logged in
            set => Preferences.Set(nameof(loginUser), value);
        }

    }
}
