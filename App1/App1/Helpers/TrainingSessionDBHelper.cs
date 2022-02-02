using App1.Database;
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

        public bool DeleteOnlyTrainingSession(TrainingSession tsession)
        { 
            if (newConnection.Delete(tsession) != 0)
                return true;
            else return false;
        }


        public bool DeleteTrainingSession(TrainingSession tsession)
        {
            picTimeDBHelper.DeleteAllPicTimesbyTrainingSession(tsession);
            if (newConnection.Delete(tsession) != 0)
                return true;
            else return false;
        }

        public bool DeleteAllTrainingSessionbyUser(User user)
        {

            var data = newConnection.Table<TrainingSession>();
            try
            {
                if (data.Delete(x => x.UserID == user.UserID) != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public int DeleteAllTrainingSessions()
        {
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

        /// <summary>
        /// Get a specic List of Trainingsession by User,by Order, List length is session count
        /// </summary>
        /// <param name="user">User Object</param>
        /// <param name="sessions">Number of Session you wanna get</param>
        /// <param name="isAscending">Order of Session (Asceding = old to new)(Descending = new to old) </param>
        /// <returns></returns>
        public List<TrainingSession> getListNumberOfTrainingSessionByUserANDOrder(User user, int sessions, bool isAscending)
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

/*        public TrainingSession getLastTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).LastOrDefault();
        }

        public List<TrainingSession> GetLastTwoTrainingSessions(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).Take(2).ToList();
        }
        public List<TrainingSession> getLastThreeTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).Take(3).ToList();
        }

        public List<TrainingSession> getLastSevenTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).Take(7).ToList();
        }

        public List<TrainingSession> getFirstThreeTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderBy(x => x.SessionId).Take(3).ToList();
        }

        public List<TrainingSession> getFirstSevenTrainingSessionbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).Take(7).ToList();
        }*/

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
                return list.FindAll(x => x.IsTrainingCompleted = true);
            }
            else
            {
                var list = newConnection.Table<TrainingSession>().Where(x => x.UserID == user.UserID).OrderByDescending(x => x.SessionId).ToList();
                return list.FindAll(x => x.IsTrainingCompleted = true);
            }
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

        //------------------------------------------------------Other GET-----------------------------------------------------------------------------------------------

        //GetAllUnsentTrainingSession for all User
        public List<TrainingSession> GetAllUnsentTrainingSessions()
        {
            return newConnection.Table<TrainingSession>().Where(x => x.IsDataSent == false).ToList();
        }

        //GetAllUnsentTrainingSession for by specific User
        public List<TrainingSession> GetUnsentTrainingSessionsbyUser(User user)
        {
            return newConnection.Table<TrainingSession>().Where(x=>x.UserID == user.UserID && x.IsDataSent == false).ToList();
        }





        //------------------------------------------------------Not My Methods-----------------------------------------------------------------------------------------------
        public List<TrainingSession> GetTrainingSessions()
        {
            //int i = 0;
            //DateTime dateTime = DateTime.Today.Date;

            //var list = from tSession in newConnection.Table<TrainingSession>()
            //           orderby tSession.SessionDate descending
            //           select tSession;
            //foreach(TrainingSession trainingSession in list)
            //{

            //}
            //return list;

            return (from tSession in newConnection.Table<TrainingSession>()
                    orderby tSession.SessionDate descending
                    select tSession).ToList();
        }


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

        public void setDataSent(int id)
        {
            var returnedTrainingSession = (from tSession in newConnection.Table<TrainingSession>()
                                           where tSession.SessionId == id
                                           select tSession).Single();
            try
            {
                if (returnedTrainingSession != null)
                {
                    returnedTrainingSession.IsDataSent = true;
                    newConnection.Update(returnedTrainingSession);
                }
            }
            catch (Exception)
            {
                throw;
            }
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
            Console.WriteLine(trainingSession.IsDataSent);
        }
        public int DeleteAllTSession()
        {
            return newConnection.DeleteAll<TrainingSession>();
        }
    }
}
