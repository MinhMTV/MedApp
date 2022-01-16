using App1.Database;
using App1.Models;
using App1.Methods;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.Helpers
{
    public class UserDBHelper
    {
        private SQLiteConnection newConnection;
        public Stringmethods stringmethods;

        public UserDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<User>(); // Create table if not exists
        }

        //Add User if user already exist by username return false
        public bool AddUser(User user, string username)
        {
            if (CheckUserexist(username))
            {
                    return false;
            }
            else
            {
                newConnection.Insert(user);
                return true;
            }
        }

        //check if user already exist in table by name
        public bool CheckUserexist(string username)
        {
            var data = newConnection.Table<User>();
            var d1 = data.Where(x => x.Username == username).FirstOrDefault();
            if (d1 != null)
            {
                return true;
            }
            else
                return false;
        }


        /* 
         Check, if the a User Table already has entries in the database if not, no User are registered 
        */
        public bool IsRegisteredUserExists()
        {
            if (newConnection.Table<User>().Count() > 0)
            {
                return true;
            }
            return false;     
        }

        /// <summary>
        /// check username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>bool true if correct else false</returns>
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

        /// <summary>
        /// Log user by username
        /// check first if exist then set preferences to username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>true if login was succesful else false</returns>
        public bool LogInUser(string username)
        {

            var data = newConnection.Table<User>();
            if(CheckUserexist(username))
            {
                Preferences.Set(constants.loginUser, username);
                return true;
            } else
            {
                return false;
            }   
        }

        /// <summary>
        /// log out user by pref login user
        /// </summary>
        public async void LogOutUser()
        {
            var data = newConnection.Table<User>();
            string userName;

            //Make sure any logged in user exists
            if (!Preferences.Get(constants.loginUser,"false").Equals("false"))
            {
                userName = Preferences.Get(constants.loginUser, "false");
                try
                {
                    await App.Current.MainPage.DisplayAlert("Erfolg", "Sie wurden ausgeloggt", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Fehler", ex.ToString(), "OK");

                }

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Achtung", "Es gibt keine eingeloggten Benutzer", "OK");
            }
        }

        //get first user on user table
        public User GetAnyUser()
        {
            return newConnection.Table<User>().First();
        }

        //get Userobj of current logged User
        public User GetLoggedUser()
        {
            try
            {
                foreach(User item in newConnection.Table<User>())
                {
                    if(item.Username == Preferences.Get(constants.loginUser,"false"))
                    {
                        return item;
                    } 
                } 
                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// get userobj by username 
        /// </summary>
        /// <param name="username"></param>
        /// <returns>userobj</returns>
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

        // Get the loggedin user property
        public string getLoggedinUserProperty(string property)
        {
            if (!Preferences.Get(constants.loginUser, "false").Equals("false"))
            {
                Console.WriteLine(Preferences.Get(constants.loginUser, "false"));

                var username = Preferences.Get(constants.loginUser, "false");
                var data = newConnection.Table<User>();
                try
                {
                    var returnedUser = (from values in data
                                        where values.Username == username
                                        select values).Single();

                    if (returnedUser != null)
                    {
                        switch (property.Trim().ToLower())
                        {
                            case "username":
                                return returnedUser.Username;
                                 
                            case "firstname":
                                return returnedUser.FirstName;
                            case "lastname":
                                return returnedUser.LastName;
                            case "password":
                                return returnedUser.Password;
                            case "email":
                                return returnedUser.Email;
                            case "userid":
                                return returnedUser.UserID.ToString();
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// change password of user by username
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SetUserPassword(string username, string password)
        {
            var data = newConnection.Table<User>();
            var updateableUser = (from values in data
                                  where values.Username == username
                                  select values).Single();
            if (updateableUser != null)
            {
                updateableUser.Password = password;
                newConnection.Update(updateableUser);
                return true;
            }
            return false;
        }

        /// <summary>
        /// delete all User in User Table
        /// </summary>
        /// <returns></returns>
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
