namespace App1.Models
{
    public static class constants
    {
        /*Shared Preferences
         * String Value means key name 
         */
        public static string loginUser { get { return "loginUser";} } //check in preferences if a user was loggin, if not default value is false
        
        public static int minID { get { return 10000; } } //set range of UserID

        public static int maxId { get { return 99999; } }

        public static string isAscending { get { return "True"; } } //set default sort by ascending
        public static string OrderBy { get { return ""; } } //set order of usercollection, default is empty string, which leads to userdbid
    }
}
