using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace CBMTraining.Models
{
    public class Admin
    {
        [PrimaryKey, AutoIncrement]
        public int AdminID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Entity { get; set; }
        public DateTime CreatedAt { get; set; }

        public Admin()
        {
        }
        public Admin(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}

