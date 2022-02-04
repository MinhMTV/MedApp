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
        public int UserID { get; set; } //unique random id for showing
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }

        public DateTime FirstSession { get; set; } //first trainingsession
        public DateTime LastSession { get; set; } //last Trainingsession
        public DateTime CreatedAt { get; set; } //when user was created

        public User()
        {
        }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public string Fullname => FirstName + " " + LastName;

    }
}
