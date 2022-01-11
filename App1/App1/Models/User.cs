using SQLite;
using System;

namespace App1.Models
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public bool IsUserLoggedIn { get; set; }
        public bool IsUserIdUpdated { get; set; }
        public bool IsUserAskedForDataProtection { get; set; }
        public bool IsDataProtectionAccepted { get; set; }
        public bool IsToDataAutoSend { get; set; }
        public DateTime SessionLastUpdated { get; set; }


        public User()
        {
        }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public bool CheckInformation()
        {
            if (!this.Username.Equals("") && !this.Password.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
