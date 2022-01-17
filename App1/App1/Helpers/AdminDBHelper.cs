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
    public class AdminDBHelper
    {
        private SQLiteConnection newConnection;
        public Stringmethods stringmethods;

        public AdminDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<Admin>(); // Create table if not exists
        }

        //Add User if user already exist by username return false
        public bool AddUser(Admin user, string username)
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
            var data = newConnection.Table<Admin>();
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
            if (newConnection.Table<Admin>().Count() > 0)
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
            var data = newConnection.Table<Admin>();
            var d1 = data.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (d1 != null)
            {
                Preferences.Set(constants.loginUser, username);
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

            var data = newConnection.Table<Admin>();
            if (CheckUserexist(username))
            {
                Preferences.Set(constants.loginUser, username);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// log out user by pref login user
        /// </summary>
        public async void LogOutUser()
        {
            var data = newConnection.Table<Admin>();
            string userName;

            //Make sure any logged in user exists
            if (!Preferences.Get(constants.loginUser, "false").Equals("false"))
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
        public Admin GetAnyUser()
        {
            return newConnection.Table<Admin>().First();
        }

        //get Userobj of current logged User
        public Admin GetLoggedUser()
        {
            try
            {
                foreach (Admin item in newConnection.Table<Admin>())
                {
                    if (item.Username == Preferences.Get(constants.loginUser, "false"))
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

        // Get the loggedin user property
        public string getLoggedinUserProperty(string property)
        {
            if (!Preferences.Get(constants.loginUser, "false").Equals("false"))
            {
                Console.WriteLine(Preferences.Get(constants.loginUser, "false"));

                var username = Preferences.Get(constants.loginUser, "false");
                var data = newConnection.Table<Admin>();
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
                            case "entity":
                                return returnedUser.Entity;

                            case "firstname":
                                return returnedUser.FirstName;
                            case "lastname":
                                return returnedUser.LastName;
                            case "password":
                                return returnedUser.Password;
                            case "email":
                                return returnedUser.Email;
                            case "userid":
                                return returnedUser.AdminID.ToString();
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
            var data = newConnection.Table<Admin>();
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
            return newConnection.DeleteAll<Admin>();
        }

        public void PrintUser(Admin user)
        {
            Console.WriteLine(user.Email);
            Console.WriteLine(user.FirstName);
            Console.WriteLine(user.LastName);
            Console.WriteLine(user.Password);
            Console.WriteLine(user.Entity);
        }
    }
}
