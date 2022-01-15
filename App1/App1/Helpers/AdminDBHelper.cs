using App1.Database;
using App1.Models;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace App1.Helpers
{
    public class AdminDBHelper
    {
        private SQLiteConnection newConnection;

        public AdminDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<Admin>();   //create table if not exists
            Console.WriteLine("Reading data");


        }
        public bool AddAdmin(Admin admin, string username)
        {
            if (CheckUserexist(username))
            {
                return false;
            }
            else
            {
                newConnection.Insert(admin);
                return true;
            }
        }
        public bool CheckUserexist(string username)
        {
            var data = newConnection.Table<Admin>();
            var d1 = data.Where(x => x.Username == username).FirstOrDefault();
            if (d1 != null)
            {
                return true;
            }
            else
                return false;

        }
        /* Check if user exist
         Check, if there is a User Table already the database if not, no User is registered 
        */
        public bool IsRegisteredUserExists()
        {
            if (newConnection.Table<Admin>().Count() > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsLoggedInUserExists()
        {
            Admin user = GetUser();
            return user.IsUserLoggedIn;
        }
        public bool ValidateLogin(string username, string password)
        {
            var data = newConnection.Table<Admin>();
            var d1 = data.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (d1 != null)
            {
                return true;
            }
            else
                return false;
        }
        public bool LogInUser(string username)
        {
            var data = newConnection.Table<Admin>();

            // Find nr of current logged in users
            var nrOfLoggedInUser = (from values in data
                                    where values.IsUserLoggedIn == true
                                    select values).Count();

            // Current logged in user should be zero
            if (nrOfLoggedInUser == 0)
            {
                var userToLogIn = (from values in data
                                   where values.Username == username
                                   select values).Single();
                userToLogIn.IsUserLoggedIn = true;

                var updateStatus = newConnection.Update(userToLogIn);
                if (updateStatus == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async void LogOutUser()
        {
            var data = newConnection.Table<Admin>();
            string userName;

            //Make sure any logged in user exists
            if (IsLoggedInUserExists())
            {

                // Get logged in user name
                userName = GetLoggedInUserName();
                var userToLogOut = (from values in data
                                    where values.Username == userName
                                    select values).Single();
                userToLogOut.IsUserLoggedIn = false;

                var updateStatus = newConnection.Update(userToLogOut);

                if (updateStatus != 0)
                {
                    await App.Current.MainPage.DisplayAlert("Erfolg", "Sie werden ausgeloggt", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Fehler", "Ausloggen nicht möglich", "OK");
                }

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Achtung", "Es gibt keine eingeloggten Benutzer", "OK");
            }
        }
        public Admin GetAnyUser()
        {
            return newConnection.Table<Admin>().First();
        }
        public Admin GetUser()
        {
            try
            {
                foreach (Admin item in newConnection.Table<Admin>())
                {
                    Console.WriteLine(item.Username);
                    Console.WriteLine(item.IsUserLoggedIn);   
                    if (item.IsUserLoggedIn == true)
                    {
                        return item;
                    }
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public Admin GetUserByName(string username)
        {
            var data = newConnection.Table<Admin>();
            try
            {
                var user = (from values in data
                            where values.Username == username
                            select values).Single();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        // Get the loggedin user name
        public string GetLoggedInUserName()
        {
            if (IsRegisteredUserExists())
            {
                var data = newConnection.Table<Admin>();
                try
                {
                    var updatableUser = (from values in data
                                         where values.IsUserLoggedIn == true
                                         select values).Single();

                    if (updatableUser != null)
                    {
                        return updatableUser.Username;
                    }

                }
                catch (Exception)
                {
                }
            }
            return string.Empty;
        }
        public string GetLoggedInUserFirstName()
        {
            if (IsRegisteredUserExists())
            {
                var data = newConnection.Table<Admin>();
                try
                {
                    var returnedUser = (from values in data
                                        where values.IsUserLoggedIn == true
                                        select values).Single();

                    if (returnedUser != null)
                    {
                        return returnedUser.FirstName;
                    }

                }
                catch (Exception)
                {
                }
            }
            return string.Empty;
        }
        // Get the loggedin user name


        public string GetLoggedInUserEmail()
        {
            if (IsRegisteredUserExists())
            {
                var data = newConnection.Table<Admin>();
                var updatableUser = (from values in data
                                     where values.IsUserLoggedIn == true
                                     select values).Single();

                if (updatableUser != null)
                {
                    return updatableUser.Email;
                }
            }

            return string.Empty;
        }
        // Get the loggedin user name
        public int GetUserID()
        {
            var data = newConnection.Table<Admin>();
            var updatableUser = (from values in data
                                 where values.IsUserLoggedIn == true
                                 select values).Single();
            if (updatableUser != null)
            {
                return updatableUser.AdminID;
            }
            return 0;
        }

        public string GetUserPassword()
        {
            var data = newConnection.Table<Admin>();
            var returnedUser = (from values in data
                                where values.IsUserLoggedIn == true
                                select values).Single();
            if (returnedUser != null)
            {
                return returnedUser.Password;
            }
            return String.Empty;
        }

        public bool SetUserPassword(string password)
        {
            var data = newConnection.Table<Admin>();
            var updateableUser = (from values in data
                                  where values.IsUserLoggedIn == true
                                  select values).Single();
            if (updateableUser != null)
            {
                updateableUser.Password = password;
                newConnection.Update(updateableUser);
                return true;
            }
            return false;
        }

        public int DeleteAllUser()
        {
            return newConnection.DeleteAll<Admin>();
        }

        public void PrintUser(Admin user)
        {
            Console.WriteLine(user.Email);
        }
    }
}
