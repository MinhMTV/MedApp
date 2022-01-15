using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace App1.Models
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
        public bool IsUserLoggedIn { get; set; }

        public Admin()
        {
        }
        public Admin(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }


        //soll gucken, ob Passwort und Username leer sind (wird aber bei der registrierungsklasse schon abgefragt)
        /* public bool CheckInformation()
        {
            if (!this.Username.Equals("") && !this.Password.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        } */
    }
}

