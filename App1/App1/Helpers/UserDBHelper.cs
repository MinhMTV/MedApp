using App1.Database;
using App1.Models;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace App1.Helpers
{
    public class UserDBHelper
    {
        private SQLiteConnection newConnection;

        public UserDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<User>(); // Create table if not exists
        }
        public bool AddUser(User user)
        {
            if (newConnection.Table<User>().Count() > 0)
            {
                DeleteAllUser();
                return false;
            }
            else
            {
                newConnection.Insert(user);
                return true;
            }
        }
        public bool IsRegisteredUserExists()
        {
            if (newConnection.Table<User>().Count() > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsLoggedInUserExists()
        {
            User user = GetUser();
            return user.IsUserLoggedIn;
        }
        public bool ValidateLogin(string username, string password)
        {
            var data = newConnection.Table<User>();
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
            var data = newConnection.Table<User>();

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
            var data = newConnection.Table<User>();
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
        public User GetAnyUser()
        {
            return newConnection.Table<User>().First();
        }
        public User GetUser()
        {
            try
            {
                User user = newConnection.Table<User>().First();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public User GetUserByName(string username)
        {
            var data = newConnection.Table<User>();
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
                var data = newConnection.Table<User>();
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
                var data = newConnection.Table<User>();
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
                var data = newConnection.Table<User>();
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
            var data = newConnection.Table<User>();
            var updatableUser = (from values in data
                                 where values.IsUserLoggedIn == true
                                 select values).Single();
            if (updatableUser != null)
            {
                return updatableUser.UserID;
            }
            return 0;
        }

        public string GetUserPassword()
        {
            var data = newConnection.Table<User>();
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
            var data = newConnection.Table<User>();
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
            return newConnection.DeleteAll<User>();
        }
        public bool UpdateUserID(int uID)
        {
            var data = newConnection.Table<User>();
            var updatableUser = (from values in data
                                 where values.UserID == 0
                                 select values).Single();
            try
            {
                if (uID > 1)
                {
                    updatableUser.UserID = uID;
                    updatableUser.IsUserIdUpdated = true;
                    newConnection.Update(updatableUser);
                    return true;
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }

            return false;
        }
        public bool UpdateDataPrptectionInformation(bool isAccepted)
        {
            var data = newConnection.Table<User>();
            var updatableUser = (from values in data
                                 where values.IsUserLoggedIn == true
                                 select values).Single();
            if (updatableUser != null)
            {
                updatableUser.IsDataProtectionAccepted = isAccepted;
                updatableUser.IsUserAskedForDataProtection = true;
                if (newConnection.Update(updatableUser) == 1)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public bool IsUserAskedForDataProtection(string username)
        {
            var data = newConnection.Table<User>();
            var user = (from values in data
                        where values.Username == username
                        select values).Single();
            return user.IsUserAskedForDataProtection;
        }
        public bool IsUserIdUpdated()
        {
            try
            {
                var data = newConnection.Table<User>();
                var user = (from values in data
                            select values).Single();
                return user.IsUserIdUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return false;
        }
        public bool IsDataProtectionAccepted(string username)
        {
            var data = newConnection.Table<User>();
            var user = (from values in data
                        where values.Username == username
                        select values).Single();
            return user.IsDataProtectionAccepted;
        }
        public void UpdateDataAutoSend(bool decison)
        {
            var data = newConnection.Table<User>();
            var updatableUser = (from values in data
                                 where values.IsUserLoggedIn == true
                                 select values).Single();
            try
            {
                if (updatableUser != null)
                {
                    updatableUser.IsToDataAutoSend = decison;
                    newConnection.Update(updatableUser);
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }
        }
        public void PrintUser(User user)
        {
            Console.WriteLine(user.Email);
        }
    }
}
