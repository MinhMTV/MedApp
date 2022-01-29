using App1.Database;
using App1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace App1.Helpers
{
    public class TrainingSessionDBHelper
    {
        private SQLiteConnection newConnection;

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

        public bool DeleteTrainingSession(object tsession)
        {
            if (newConnection.Delete(tsession) != 0)
                return true;
            else return false;
        }

        public bool DeleteTrainingSessionbyUser(User user)
        {




        }


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

        public List<TrainingSession> GetUnsentTrainingSessions()
        {
            List<TrainingSession> trainingSessions = new List<TrainingSession>();

            trainingSessions = (from tSession in newConnection.Table<TrainingSession>()
                                where tSession.IsDataSent == false
                                select tSession).ToList<TrainingSession>();

            return trainingSessions;
        }

        public List<TrainingSession> GetLastTwoTrainingSessions()
        {
            return newConnection.Table<TrainingSession>().OrderByDescending(x => x.SessionId).Take(2).ToList();
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


        public void PrintTrainingSession(TrainingSession trainingSession)
        {
            Console.WriteLine(trainingSession.SessionId);
            Console.WriteLine(trainingSession.SessionDate);
            Console.WriteLine(trainingSession.PatientId);
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
