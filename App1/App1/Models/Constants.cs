namespace App1.Models
{
    public static class constants
    {
        /*Shared Preferences
         * String Value means key name 
         */
        public const string loginUser = "loginUser";//check in preferences if a user was loggin, if not default value is false
        public const string isAscending = "IsAscending"; //set default sort by ascending
        public const string OrderBy = "OrderBy";  //set order of usercollection, default is empty string, which leads to userdbid

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

        //admin
        public const string entity = "entity";

        //pictures
        public const string imageid = "imageid";
        public const string imagetype = "imagetype";
        public const string imagepath = "imagepath";
        public const string typebad = "bad";
        public const string typegood = "good";

        public const string ImageFolderPath = "App1.EmbeddedImages.";



        //constant user settings

        public const int minID = 10000;  //set range of UserID

        public const int maxId = 99999;


        


    }
}
