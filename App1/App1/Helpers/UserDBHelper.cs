using App1.Database;
using App1.Models;
using App1.Methods;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Extensions;
using System.Collections.ObjectModel;

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

        //------------------------------------------------------DB MANIPULATION METHODS-----------------------------------------------------------------------------------------------

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
        public bool DeleteUser(object user)
        {
            if (newConnection.Delete(user) != 0)
                return true;
            else return false;
        }

/*        public object checktype(object obk)
        {
            var data = newConnection.Table<object>();
            return
        }*/

        public bool DeleteUserbyName(string username)
        {
            if (CheckUserexist(username))
            {
                return false;
            }
            else
            {
                var data = newConnection.Table<User>();      
                var user = (from values in data
                            where values.Username == username
                                      select values).Single();
                var succes = newConnection.Delete(user);
                if(succes == 1)
                {
                    return true;
                } else
                {
                    Console.WriteLine("User couldnt be deleted");
                    return false;
                }
            }
        }

        /// <summary>
        /// delete all User in User Table
        /// </summary>
        /// <returns></returns>
        public int DeleteAllUser()
        {
            return newConnection.DeleteAll<User>();
        }

        /// <summary>
        /// change password of user by username
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">old password</param>
        /// <returns>true if new password was set succesfully</returns>
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

        public void UpdateDataAutoSend(bool decison)
        {
            var username = Preferences.Get(constants.loginUser, "false");
            var data = newConnection.Table<User>();
            var updatableUser = (from values in data
                                 where values.Username == username
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

        //update Information wheter user accepted data protection or not
        public bool UpdateDataPrptectionInformation(bool isAccepted)
        {
            var data = newConnection.Table<User>();
            var username = Preferences.Get(constants.loginUser, "false");
            if (username != "false")
            {
                var updatableUser = (from values in data
                                     where values.Username == username
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
            else return false;
        }



        //------------------------------------------------------DB SEARCH METHODS-----------------------------------------------------------------------------------------------
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

        /* 
         Check, if the a user by username alreay accepted the dataprotection
        */
        public bool IsDataProtectionAccepted(string username)
        {
            var data = newConnection.Table<User>();
            var user = (from values in data
                        where values.Username == username
                        select values).Single();
            return user.IsDataProtectionAccepted;
        }

        /* 
         Check, if the a user by username was already asked for data protection
        */
        public bool IsUserAskedForDataProtection(string username)
        {
            var data = newConnection.Table<User>();
            var user = (from values in data
                        where values.Username == username
                        select values).Single();
            return user.IsUserAskedForDataProtection;
        }
        //check if userid is unique and return true if so
        public bool IsUserIDUnique(int userid)
        {
            var data = newConnection.Table<User>();
            var d1 = data.Where(x => x.UserID == userid).FirstOrDefault();
            if (d1 == null)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        //------------------------------------------------------DB GET METHODS-----------------------------------------------------------------------------------------------

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
                foreach (User item in newConnection.Table<User>())
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
        /// <param name=constants.susername></param>
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
        public User GetUserByUserID(int userid)
        {
            var data = newConnection.Table<User>();
            try
            {
                var user = (from values in data
                            where values.UserID == userid
                            select values).Single();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User GetUserByUserDBID(int userdbid)
        {
            var data = newConnection.Table<User>();
            try
            {
                var user = (from values in data
                            where values.UserID == userdbid
                            select values).Single();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //get all User as a List of User Objects
        public List<User> GetAllUserToList1()
        {
            var user = new List<User>();
            var data = newConnection.Table<User>();
            foreach (var item in data)
            {
                user.Add(item);
            }
            user.ForEach(x => Console.WriteLine(x.Username));
            return user;
        }
        public List<User> GetAllUserToList()
        {
            var userList = newConnection.Table<User>().ToList();
            return userList;
        }

        //get all user as Collection
        public ObservableCollection<User> GetAllUserToCollection()
        {
            var userList = GetAllUserToList();
            ObservableCollection<User> UserCollection = new ObservableCollection<User>();

           foreach (var item in userList)
            {
                UserCollection.Add(item);
            }
           return UserCollection;
        }

        //get all user by Collection by inserting UserList
        public ObservableCollection<User> GetAllUserByListToCollection (List<User> users)
        {
            ObservableCollection<User> UserCollection = new ObservableCollection<User>();
            foreach (var item in users)
            {
                UserCollection.Add(item);
            }
            return UserCollection;
        }

        /// <summary>
        /// get all User By Collection by specific order
        /// </summary>
        /// <param name="order">insert String like username, email...</param>
        /// <param name="isAscending">true = sort order Ascending, false = Descending</param>
        /// <returns></returns>
        public ObservableCollection<User> GetAllUserByOrder(String order, bool isAscending)
        {
            List<User> userList = GetAllUserToListByOrder(order, isAscending);
            return GetAllUserByListToCollection(userList);
        }

        public List<User> GetAllUserToListByOrder(String order,bool isAscending)
        {
            var data = newConnection.Table<User>();

            if (isAscending)
            {
                switch (order)
                {
                    case constants.username:
                        return data.OrderBy(x => x.Username).ToList();
                    case constants.email:
                        return data.OrderBy(x => x.Email).ToList();
                    case constants.userid:
                        return data.OrderBy(x => x.UserID).ToList();
                    case constants.firstname:
                        return data.OrderBy(x => x.FirstName).ToList();
                    case constants.lastname:
                        return data.OrderBy(x => x.LastName).ToList();
                    case constants.age:
                        return data.OrderBy(x => x.Age).ToList();
                    case constants.createdat:
                        return data.OrderBy(x => x.CreatedAt).ToList();
                    case constants.IsDataProtectionAccepted:
                        return data.OrderBy(x => x.IsDataProtectionAccepted).ToList();
                    case constants.IsToDataAutoSend:
                        return data.OrderBy(x => x.IsToDataAutoSend).ToList();
                    case constants.firstsession:
                        return data.OrderBy(x => x.FirstSession).ToList();
                    case constants.lastsession:
                        return data.OrderBy(x => x.LastSession).ToList();
                    case constants.sessionlastupdated:
                        return data.OrderBy(x => x.SessionLastUpdated).ToList();
                    case constants.start:
                        return data.OrderBy(x => x.Start).ToList();
                    case constants.end:
                        return data.OrderBy(x => x.End).ToList();
                    case constants.minutes:
                        return data.OrderBy(x => x.SessionTimeMin).ToList();
                    case constants.seconds:
                        return data.OrderBy(x => x.SessionTimeSec).ToList();
                    default:
                        return data.OrderBy(x => x.UserDBID).ToList(); ;

                }
            }
            else
            {
                switch (order)
                {
                    case constants.username:
                        return data.OrderByDescending(x => x.Username).ToList();
                    case constants.email:
                        return data.OrderByDescending(x => x.Email).ToList();
                    case constants.userid:
                        return data.OrderByDescending(x => x.UserID).ToList();
                    case constants.firstname:
                        return data.OrderByDescending(x => x.FirstName).ToList();
                    case constants.lastname:
                        return data.OrderByDescending(x => x.LastName).ToList();
                    case constants.age:
                        return data.OrderByDescending(x => x.Age).ToList();
                    case constants.createdat:
                        return data.OrderByDescending(x => x.CreatedAt).ToList();
                    case constants.IsDataProtectionAccepted:
                        return data.OrderByDescending(x => x.IsDataProtectionAccepted).ToList();
                    case constants.IsToDataAutoSend:
                        return data.OrderByDescending(x => x.IsToDataAutoSend).ToList();
                    case constants.firstsession:
                        return data.OrderByDescending(x => x.FirstSession).ToList();
                    case constants.lastsession:
                        return data.OrderByDescending(x => x.LastSession).ToList();
                    case constants.sessionlastupdated:
                        return data.OrderByDescending(x => x.SessionLastUpdated).ToList();
                    case constants.start:
                        return data.OrderByDescending(x => x.Start).ToList();
                    case constants.end:
                        return data.OrderByDescending(x => x.End).ToList();
                    case constants.minutes:
                        return data.OrderByDescending(x => x.SessionTimeMin).ToList();
                    case constants.seconds:
                        return data.OrderByDescending(x => x.SessionTimeSec).ToList();
                    default:
                        return data.OrderByDescending(x => x.UserDBID).ToList(); ;

                }
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
                            case constants.userdbid:
                                return returnedUser.UserDBID.ToString();
                            case constants.username:
                                return returnedUser.Username;
                            case constants.email:
                                return returnedUser.Email;
                            case constants.userid:
                                return returnedUser.UserID.ToString();
                            case constants.firstname:
                                return returnedUser.FirstName;
                            case constants.lastname:
                                return returnedUser.LastName;
                            case constants.password:
                                return returnedUser.Password;
                            case constants.IsUserIdUpdated:
                                return returnedUser.IsUserIdUpdated.ToString();
                            case constants.age:
                                return returnedUser.Age.ToString();
                            case constants.createdat:
                                return returnedUser.CreatedAt.ToString();
                            case constants.IsDataProtectionAccepted:
                                return returnedUser.IsDataProtectionAccepted.ToString();
                            case constants.IsToDataAutoSend:
                                return returnedUser.IsToDataAutoSend.ToString();
                            case constants.firstsession:
                                return returnedUser.FirstSession.ToString();
                            case constants.lastsession:
                                return returnedUser.LastSession.ToString();
                            case constants.sessionlastupdated:
                                return returnedUser.SessionLastUpdated.ToString();
                            case constants.start:
                                return returnedUser.Start.ToString();
                            case constants.end:
                                return returnedUser.End.ToString();
                            case constants.minutes:
                                return returnedUser.SessionTimeMin.ToString();
                            case constants.seconds:
                                return returnedUser.SessionTimeSec.ToString();
                            default:
                                break;
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

        //------------------------------------------------------DB COMPARE METHODS-----------------------------------------------------------------------------------------------
        /// <summary>
        /// check username and password and login user 
        /// </summary>
        /// <param name=username> </param>
        /// <param name=password> </param>
        /// <returns>bool true if correct else false</returns>
        public bool ValidateLogin(string username, string password)
        {
            var data = newConnection.Table<User>();
            var d1 = data.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (d1 != null)
            {
                Preferences.Set(constants.loginUser, username);
                return true;
            }
            else
                return false;
        }



        //------------------------------------------------------DB USE METHODS-----------------------------------------------------------------------------------------------
        /// <summary>
        /// Log user by only username
        /// check first if exist then set preferences to username
        /// </summary>
        /// <param name=username></param>
        /// <returns>true if login was succesful else false</returns>
        public bool LogInUser(string username)
        {

            var data = newConnection.Table<User>();
            var d1 = data.Where(x => x.Username == username ).FirstOrDefault();
            if (CheckUserexist(username))
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

        //generate random user id between 10000 - 99999
        public int GenerateUserID()
        {
            Random rand = new Random();
            var userid = rand.Next(constants.minID, constants.maxId);
            if (IsUserIDUnique(userid))
            {
                return userid;
            }
            else
            {
                return -1;
            }
        }

        //------------------------------------------------------DEBUG METHODS-----------------------------------------------------------------------------------------------
        public void PrintUser(User user)
        {
            var message =
                "Username " + user.Username +
                "\nUserdbid " + user.UserDBID.ToString() +
                "\nEmail " + user.Email +
                "\nFirstname " + user.FirstName +
                "\nLastname " + user.LastName +
                "\nPassword " + user.Password +
                "\nUserID " + user.UserID +
                "\nisUseridupdated " + user.IsUserIdUpdated +
                "\nisUserAskedforDataprotection " + user.IsUserAskedForDataProtection +
                "\nisDataAutoSend " + user.IsToDataAutoSend +
                "\nsessionlastupdated " + user.SessionLastUpdated;
            Console.WriteLine(message);
        }

        public void PrintAllUser()
        {
            var data = newConnection.Table<User>();
            foreach (var user in data)
            {
                var message =
                "Username " + user.Username +
                "\nUserdbid " + user.UserDBID.ToString() +
                "\nEmail " + user.Email +
                "\nFirstname " + user.FirstName +
                "\nLastname " + user.LastName +
                "\nPassword " + user.Password +
                "\nUserID " + user.UserID +
                "\nisUseridupdated " + user.IsUserIdUpdated +
                "\nisUserAskedforDataprotection " + user.IsUserAskedForDataProtection +
                "\nisDataAutoSend " + user.IsToDataAutoSend +
                "\nsessionlastupdated " + user.SessionLastUpdated;
                Console.WriteLine(message);
            }
        }

        public async void debugUser (User user)
        {
            var message =
                "Username " + user.Username +
                "\nUserdbid " + user. UserDBID.ToString() + 
                "\nEmail " + user.Email +
                "\nFirstname " + user.FirstName +
                "\nLastname " + user.LastName +
                "\nPassword " + user.Password +
                "\nUserID " + user.UserID +
                "\nisUseridupdated " + user.IsUserIdUpdated +
                "\nisUserAskedforDataprotection " + user.IsUserAskedForDataProtection +
                "\nisDataAutoSend " + user.IsToDataAutoSend +
                "\nsessionlastupdated " + user.SessionLastUpdated;
            await App.Current.MainPage.DisplayAlert("Debug", message, "OK");

        }
    }
}
