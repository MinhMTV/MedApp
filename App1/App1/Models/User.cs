using SQLite;
using System;

namespace App1.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserDBID { get; set; } //unique id increase by number of user, also key to login
        public string Username { get; set; }
        public string Email { get; set; }

        [Indexed]
        public int UserID { get; set; } //unique random id, necessary to send user to server
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public bool IsUserIdUpdated { get; set; }
        public bool IsUserAskedForDataProtection { get; set; }
        public bool IsDataProtectionAccepted { get; set; }
        public bool IsToDataAutoSend { get; set; }
        public DateTime SessionLastUpdated { get; set; }
        public DateTime TherapieStart { get; set; }
        public DateTime TherapieEnd { get; set; }



        public User()
        {
        }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public string Fullname => FirstName + " " + LastName;

        public String LastSession => SessionLastUpdated.ToShortDateString();
    }
}
