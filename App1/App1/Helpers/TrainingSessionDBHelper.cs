using App1.Database;
using App1.Extensions;
using App1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace App1.Helpers
{
    public class TrainingSessionDBHelper
    {
        private SQLiteConnection newConnection;
        private PicTimeDBHelper picTimeDBHelper = new PicTimeDBHelper();

        public TrainingSessionDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<TrainingSession>();
        }

        //------------------------------------------------------DB MANIPULATION METHODS-----------------------------------------------------------------------------------------------


        public void AddTrainingSession(TrainingSession trainingSession)
        {
            newConnection.Insert(trainingSession);
        }

        //Delete Trainingssession without PicTime
        public bool DeleteOnlyTrainingSession(TrainingSession tsession)
        { 
            if (newConnection.Delete(tsession) != 0)
                return true;
            else return false;
        }


        //Delete Trainingssession with all PicTimes
        public bool DeleteTrainingSession(TrainingSession tsession)
        {
            picTimeDBHelper.DeleteAllPicTimesbyTrainingSession(tsession);
            if (newConnection.Delete(tsession) != 0)
                return true;
            else return false;
        }

        //Delete Trainingssession and PicTimes by User
        public bool DeleteAllTrainingSessionbyUser(User user)
        {

            var data = newConnection.Table<TrainingSession>();
            try
            {
                if (data.Delete(x => x.UserID == user.UserID) != 0)
                {
                    picTimeDBHelper.DeleteAllPicTimesbyUser(user);
                    return true;
                }
                    
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Delete Trainingssession and PicTimes and Reset Table (Delete every RECORD)
        public int DeleteAllTrainingSessions()
        {
            newConnection.DeleteAll<PicTime>();
            newConnection.DeleteAll<TrainingSession>();
            newConnection.DropTable<PicTime>();
            return newConnection.DeleteAll<TrainingSession>();
        }

        //------------------------------------------------------DB SEARCH METHODS-----------------------------------------------------------------------------------------------



        //------------------------------------------------------DB GET METHODS-----------------------------------------------------------------------------------------------

        //------------------------------------------------------Get User Specific Training-----------------------------------------------------------------------------------------------

        public TrainingSession getFirstTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).FirstOrDefault();
        }

        public TrainingSession getLastTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).LastOrDefault();
        }

        public TrainingSession getFirstCmplTrainingSessionbyUser(User user)
        {
            var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).ToList();
            return list.FindAll(x => x.IsTrainingCompleted == true).FirstOrDefault();
        }

        public TrainingSession getNextCmplTrainingbyUserANDSession(User user, TrainingSession tsession)
        {
            var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).ToList();
            list = list.FindAll(x => x.IsTrainingCompleted == true);
            return list.FindAll(x => x.cmplSession == tsession.cmplSession + 1).SingleOrDefault();
        }

        public TrainingSession getBeforeCmplTrainingbyUserANDSession(User user, TrainingSession tsession)
        {
            var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).ToList();
            list = list.FindAll(x => x.IsTrainingCompleted == true);
            return list.FindAll(x => x.cmplSession == tsession.cmplSession - 1).SingleOrDefault();
        }


        /// <summary>
        /// get Last TrainingSessionBy User
        /// </summary>
        /// <param name="user">insert user</param>
        /// <returns></returns>
        public TrainingSession getLastCmpTrainingSessionbyUser(User user)
        {
            var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).ToList();
            return list.FindAll(x => x.IsTrainingCompleted == true).LastOrDefault();
        }

        /// <summary>
        /// Get a specic List of Trainingsession by User,by Order, List length is session count
        /// </summary>
        /// <param name="user">User Object</param>
        /// <param name="sessions">Number of Session you wanna get</param>
        /// <param name="isAscending">Order of Session (Asceding = old to new)(Descending = new to old) </param>
        /// <returns></returns>
        public List<TrainingSession> getListNrSessionByUserANDOrder(User user, int sessions, bool isAscending)
        {
            if (isAscending)
            {
                return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).Take(sessions).ToList();
            }
            else
            {
                return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).Take(sessions).ToList();
            }
        }

        /// <summary>
        /// Get a specic List of Trainingsession by User,by Order, List length is session count
        /// </summary>
        /// <param name="user">User Object</param>
        /// <param name="sessions">Number of Session you wanna get</param>
        /// <param name="isAscending">Order of Session (Asceding = old to new)(Descending = new to old) </param>
        /// <returns></returns>
        public List<TrainingSession> getListNrCmplSessionByUserANDOrder(User user, int sessions, bool isAscending)
        {
            if (isAscending)
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted == true).Take(3).ToList();
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted == true).Take(3).ToList();
            }
        }

        //
        public List<TrainingSession> getAllTrainingSessionListbyUserAndOrder(User user, bool isAscending)
        {
            if(isAscending == true)
            {
                return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
            }
            else
            {
                return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).ToList();
            }
        }

        public ObservableCollection<TrainingSession> getAllTrainingSessionCollectionbyUserAndOrder(User user, bool isAscending)
        {
            var tList = getAllTrainingSessionListbyUserAndOrder(user,isAscending);
            ObservableCollection<TrainingSession> tCollection = new ObservableCollection<TrainingSession>();
            foreach (var item in tList)
            {
                tCollection.Add(item);
            }
            return tCollection;
        }

        public List<TrainingSession> getCompletedTrainingSessionListbyUserAndOrder(User user, bool isAscending)
        {
            if (isAscending == true)
            {
                var list =  newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted == true);
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).ToList();
                
                return list.FindAll(x => x.IsTrainingCompleted == true);
            }
        }

        public List<TrainingSession> getCompletedTrainingSessionListbyUserIDAndOrder(int userid, bool isAscending)
        {
            if (isAscending == true)
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == userid).OrderBy(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted == true);
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == userid).OrderByDescending(x => x.SessionId).ToList();

                return list.FindAll(x => x.IsTrainingCompleted == true);
            }
        }

        public List<TrainingSession> getInCompletedTrainingSessionListbyUserAndOrder(User user, bool isAscending)
        {
            if (isAscending == true)
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted == false);
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted == false);
            }
        }

        public List<TrainingSession> GetCompletedWeekListbyUserAndOrder(User user, bool isAscending)
        {
            Console.WriteLine(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
            if (isAscending == true)
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                list = list.FindAll(x => x.IsTrainingCompleted == true);
                
                return list.FindAll(x =>x.SessionDate >= DateTime.Now.StartOfWeek(DayOfWeek.Monday));
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).ToList();
                list = list.FindAll(x => x.IsTrainingCompleted == true);
                return list.FindAll(x => x.SessionDate >= DateTime.Now.StartOfWeek(DayOfWeek.Monday));

            }
        }

        /// <summary>
        /// get actual Completed Week of Training and Sort by Day, key 0 for Monday to 6 for sunday
        /// </summary>
        /// <param name="user">user </param>
        /// <param name="isAscending"></param>
        /// <returns>List with Key and Value pair, and empty list if no values </returns>
        public List<KeyValuePair<int, TrainingSession>> GetthisCompletedWeekbyUserAndOrderSortByDay(User user, bool isAscending)
        {
            Console.WriteLine(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
            List<KeyValuePair<int,TrainingSession>> ListDay = new List<KeyValuePair<int, TrainingSession>>();
            if (isAscending == true)
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                list = list.FindAll(x => x.IsTrainingCompleted == true);
                var startDay = DateTime.Now.StartOfWeek(DayOfWeek.Monday).Day; //set startday to last Monday as int for day
                for (int x = 0; x < 7; x++)
                {
                    var day = startDay + x;
                    var daylist = list.FindAll(x => x.SessionDate.Day == day);
                    foreach(var tsession in daylist)
                    {
                        ListDay.Add(new KeyValuePair<int, TrainingSession>(x, tsession));
                    }
                }
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                list = list.FindAll(x => x.IsTrainingCompleted == true);
                var startDay = DateTime.Now.StartOfWeek(DayOfWeek.Monday).Day; //set startday to last Monday as int for day
                for (int x = 0; x < 7; x++)
                {
                    var day = startDay + x;
                    var daylist = list.FindAll(x => x.SessionDate.Day == day);
                    foreach (var tsession in daylist)
                    {
                        ListDay.Add(new KeyValuePair<int, TrainingSession>(x, tsession));
                    }
                }

            }
            return ListDay;
        }

        /// <summary>
        /// get actual Completed Week of Training and Sort by Day,  key 0 for Monday to 6 for sunday
        /// </summary>
        /// <param name="user">user </param>
        /// <param name="isAscending"></param>
        /// <param name="day">WeekStart</param>
        /// <returns>List with Key and Value pair, and empty list if no values </returns>
        public List<KeyValuePair<int, TrainingSession>> GetCmplWeekbyUserOrderANDWeekSortByDay(User user, bool isAscending,DateTime Weekday)
        {
            List<KeyValuePair<int, TrainingSession>> ListDay = new List<KeyValuePair<int, TrainingSession>>();
            if (isAscending == true)
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                list = list.FindAll(x => x.IsTrainingCompleted == true);
                var startDay = Weekday.Date; //set startday given startday Monday
                for (int x = 0; x < 7; x++)
                {
                    var day = startDay.AddDays(x);
                    var daylist = list.FindAll(x => x.SessionDate.Date == day);
                    foreach (var tsession in daylist)
                    {
                        ListDay.Add(new KeyValuePair<int, TrainingSession>(x, tsession));
                    }
                }
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).ToList();
                list = list.FindAll(x => x.IsTrainingCompleted == true);
                var startDay = Weekday.Date; //set startday given startday Monday
                for (int x = 0; x < 7; x++)
                {
                    var day = startDay.AddDays(x);
                    var daylist = list.FindAll(x => x.SessionDate.Date == day);
                    foreach (var tsession in daylist)
                    {
                        ListDay.Add(new KeyValuePair<int, TrainingSession>(x, tsession));
                    }
                }

            }
            return ListDay;
        }



        //------------------------------------------------------Get General Training-----------------------------------------------------------------------------------------------

        public List<TrainingSession> GetAllTrainingsSessionToList()
        {
            return newConnection.Table<TrainingSession>().ToList();
        }

        public List<TrainingSession> GetLastTwoTrainingSessions()
        {
            return newConnection.Table<TrainingSession>().OrderByDescending(x => x.SessionId).Take(2).ToList();
        }

        public TrainingSession GetLastTrainingSession()
        {
            return newConnection.Table<TrainingSession>().LastOrDefault();
        }

        //get all TrainingsSession as Collection
        public ObservableCollection<TrainingSession> GetAllTrainingSessionToCollection()
        {
            var tList = GetAllTrainingsSessionToList();
            ObservableCollection<TrainingSession> tCollection = new ObservableCollection<TrainingSession>();

            foreach (var item in tList)
            {
                tCollection.Add(item);
            }
            return tCollection;
        }

        //get all TrainingsSession by Collection by inserting UserList
        public ObservableCollection<TrainingSession> GetAllTrainingSessionByListToCollection(List<TrainingSession> tsession)
        {
            ObservableCollection<TrainingSession> tCollection = new ObservableCollection<TrainingSession>();
            foreach (var item in tsession)
            {
                tCollection.Add(item);
            }
            return tCollection;
        }

        public ObservableCollection<TrainingSession> GetAllTrainingSessionToCollectionByOrder(bool isAscending)
        {
            List<TrainingSession> tList = GetAllTrainingSessionListByOrder(isAscending);
            return GetAllTrainingSessionByListToCollection(tList);
        }

        public List<TrainingSession> GetAllTrainingSessionListByOrder(bool isAscending)
        {
            var data = newConnection.Table<TrainingSession>();

            if (isAscending)
            {
                return data.OrderBy(x => x.SessionId).ToList();
            }
            else
            {
                return data.OrderByDescending(x => x.SessionId).ToList();
            }
        }

        

        //------------------------------------------------------Not My Methods-----------------------------------------------------------------------------------------------


        public List<TrainingSession> GetLastTrainingSessions()
        {
            DateTime dateTime = DateTime.Today;
            List<TrainingSession> trainingSessions = new List<TrainingSession>();

            for (int i = 0; i < 7; i++)
            {
                var trainingSession = (from tSession in newConnection.Table<TrainingSession>()
                                       where tSession.SessionDate == dateTime
                                       select tSession).ToList<TrainingSession>();

                if (trainingSession.Count == 0)
                {
                    trainingSessions.Add(new TrainingSession(dateTime));
                }
                else
                {
                    trainingSessions.AddRange(trainingSession);
                }
                dateTime = dateTime.AddDays(-1);

            }
            return trainingSessions;
            //var data = newConnection.Table<TrainingSession>().Take(7).ToList();
        }

      
      

        public void UpdateInformation(TrainingSession ts, DateTime dateTime)
        {
            ts.SessionDate = dateTime.Date;
            //if (ts.IsTrainingCompleted == true)
            //{
            //    ts.ElapsedTime = "60";
            //}
            //if (ts.IsTrainingCompleted == false)
            //{
            //    ts.IsTrainingCompleted = true;
            //}

            newConnection.Update(ts);
        }

        //------------------------------------------------------Not My Methods-----------------------------------------------------------------------------------------------
        public void PrintTrainingSession(TrainingSession trainingSession)
        {
            Console.WriteLine(trainingSession.SessionId);
            Console.WriteLine(trainingSession.SessionDate);
            Console.WriteLine(trainingSession.UserID);
            Console.WriteLine(trainingSession.NrOfGoodCorrectImages);
            Console.WriteLine(trainingSession.NrOfGoodWrongImages);
            Console.WriteLine(trainingSession.NrOfBadCorrectImages);
            Console.WriteLine(trainingSession.NrOfBadWrongImages);
            Console.WriteLine(trainingSession.ElapsedTime);
            Console.WriteLine(trainingSession.IsTrainingCompleted);
        }
        public int DeleteAllTSession()
        {
            return newConnection.DeleteAll<TrainingSession>();
        }
    }
}
