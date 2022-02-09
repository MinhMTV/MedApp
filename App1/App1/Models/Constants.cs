﻿namespace App1.Models
{
    public static class constants
    {
        //App Status
        public const string isTrainingPage = "isTrainingPage"; //check if trainingpage or not


        /*Shared Preferences
         * String Value means key name 
         */
/*        public const string loginUser = "loginUser";//check in preferences if a user was loggin, if not default value is false
        public const string isAscending = "IsAscending"; //set default sort by ascending
        public const string OrderBy = "OrderBy";  
        public const string isImageAscending = "IsImageAscending"; //set default sort for images by ascending 
        public const string isGoodImageAscending = "IsGoodImageAscending"; //set default sort for good images by ascending 
        public const string isBadImageAscending = "IsBadImageAscending"; //set default sort for bad images by ascending */




        /*User Properties*/
        public const string userdbid = "userdbid";
        public const string password = "password";
        public const string IsUserIdUpdated = "isuseridupdated";


        public const string username = "username";
        public const string email = "email";
        public const string userid = "userid"; 
        public const string firstname = "firstname";
        public const string lastname = "lastname"; 
        public const string age = "age"; 
        public const string createdat = "createdat"; 
        public const string IsDataProtectionAccepted = "isdataprotectionaccepted"; 
        public const string IsToDataAutoSend = "istodataautosend";
        public const string firstsession = "firstsession"; 
        public const string lastsession = "lastsession"; 
        public const string sessionlastupdated = "sessionlastupdated";
        public const string start = "start"; 
        public const string end = "end"; 
        public const string minutes = "minutes"; 
        public const string seconds = "seconds";
        public const string isTutorial = "isTutorial";

        //admin
        public const string entity = "entity";

        //pictures
        public const string imageid = "imageid";
        public const string imagetype = "imagetype";
        public const string imagepath = "imagepath";
        public const string typebad = "bad";
        public const string typegood = "good";

        public const string ImageFolderPath = "App1.EmbeddedImages.";

        //PopupNavigation
        public const string userPopup = "userpopup";
        public const string imagePopup = "imagepopup";
        public const string goodimagePopup = "goodimagepopup";
        public const string badimagePopup = "badimagepopup";

        //constant user settings

        public const int minID = 10000;  //set range of UserID

        public const int maxId = 99999;


        


    }
}
