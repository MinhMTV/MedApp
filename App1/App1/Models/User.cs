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
        public DateTime FirstSession { get; set; } //first trainingsession
        public DateTime LastSession { get; set; } //last Trainingsession
        public DateTime SessionLastUpdated { get; set; } //time, where session was last send to database online
        public DateTime CreatedAt { get; set; } //when user was created
        public DateTime Start { get; set; } //therapystart (only adjust by admin)
        public DateTime End { get; set; } //therapyend(only adjust by admin)
        public int SessionTimeMin { get; set; }

        public int SessionTimeSec { get; set; }

        //therapyend/start = time of therapy, where user can login 


        public User()
        {
        }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public string Fullname => FirstName + " " + LastName;

        public String SessionLastUpdatedshort => SessionLastUpdated.ToShortDateString();
    }
}
