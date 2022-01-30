using App1.Database;
using App1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App1.Helpers
{
	public class PicTimeDBHelper : ContentPage
	{

		private SQLiteConnection newConnection;
		public PicTimeDBHelper ()
		{
			newConnection = DependencyService.Get<ISQLite>().GetConnection();
			newConnection.CreateTable<PicTime>();
		}

		//------------------------------------------------------DB MANIPULATION METHODS-----------------------------------------------------------------------------------------------

		public void AddPicTime(PicTime pictime)
		{
			newConnection.Insert(pictime);
		}

		public bool DeletePicTime(object pictime)
		{
			if (newConnection.Delete(pictime) != 0)
				return true;
			else return false;
		}

		public bool DeleteAllPicTimesbyTrainingSession(TrainingSession tsession)
		{
			var data = newConnection.Table<PicTime>();
			try
			{
				if (data.Delete(x => x.SessionID == tsession.SessionId) != 0)
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

		public bool DeleteAllPicTimesbyUser(User user)
        {
			var data = newConnection.Table<PicTime>();
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

		public int DeleteAllPicTime()
		{
			return newConnection.DeleteAll<PicTime>();
		}

		//------------------------------------------------------DB SEARCH METHODS-----------------------------------------------------------------------------------------------

		public List<PicTime> getAllPictimebyTrainingSession(TrainingSession tsession)
		{
			return newConnection.Table<PicTime>().Where(x => x.SessionID == tsession.SessionId).ToList();
		}

		public List<PicTime> getAllPictimebyUser(User user)
		{
			return newConnection.Table<PicTime>().Where(x => x.UserID == user.UserID).ToList();
		}

		public List<PicTime> getAllPictimebySessionID(int id)
        {
			return newConnection.Table<PicTime>().Where(x => x.SessionID == id).ToList();
		}

		public List<PicTime> GetAllPictimeToList()
		{
			return newConnection.Table<PicTime>().ToList();
		}

		public List<PicTime> GetAllPictimeToListByType(PicType picType)
		{
			return newConnection.Table<PicTime>().Where(x=> x.Type == picType).ToList();
		}

	}
}