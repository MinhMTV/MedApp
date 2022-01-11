using App1.Database;
using App1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace App1.Helpers
{
    class PictureDBHelper
    {
        private SQLiteConnection newConnection;

        public PictureDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<Pictures>(); // Create table if not exists
            AddPictures();
        }

        /*
         * Creates a list with all picures
         * 
         */

        public List<Pictures> InitializeFileNames()
        {
            List<Pictures> newList = new List<Pictures>();
            for (int i = 1; i <= GlobalVariables.NroOfAvailablePics; i++)
            {
                if (i <= GlobalVariables.NrOfAvailableGoodPics)
                {
                    Pictures picture = new Pictures { TypeId = i, Type = PicType.Good, Photo = "g" + i.ToString("00") + ".jpg" };
                    newList.Add(picture);
                }
                else
                {
                    Pictures picture = new Pictures { TypeId = i, Type = PicType.Bad, Photo = "b" + i.ToString("00") + ".jpg" };
                    newList.Add(picture);
                }
            }
            return newList;
        }

        public void AddPictures()
        {
            //newConnection.DeleteAll<Picture>();
            int val = newConnection.Table<Pictures>().Count();

            if (newConnection.Table<Pictures>().Count() != GlobalVariables.NroOfAvailablePics)
            {
                List<Pictures> pictureList = InitializeFileNames();
                foreach (Pictures picture in pictureList)
                {
                    newConnection.Insert(picture);
                }
            }

        }

        public List<Pictures> GetPictures()
        {
            List<Pictures> sortedPictures = (from picture in newConnection.Table<Pictures>() select picture).ToList();
            List<Pictures> randomPictures = new List<Pictures>();
            List<int> randomNumbers = new List<int>();
            Random random = new Random();

            for (int i = 0; i < GlobalVariables.NroOfAvailablePics; i++)
            {
                int number = random.Next(0, GlobalVariables.NroOfAvailablePics);
                while (randomNumbers.Contains(number))
                {
                    number = random.Next(0, GlobalVariables.NroOfAvailablePics);
                }

                randomNumbers.Add(number);
                randomPictures.Add(sortedPictures[number]);
            }
            return randomPictures;
        }

    }
}
