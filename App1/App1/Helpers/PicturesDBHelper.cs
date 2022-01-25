using App1.Database;
using App1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace App1.Helpers
{
    class PictureDBHelper
    {
        private SQLiteConnection newConnection;

        public PictureDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<Pictures>(); // Create table if not exists
 //           AddPictures();
        }

        //------------------------------------------------------DB MANIPULATION METHODS-----------------------------------------------------------------------------------------------

        //Add Image Path if user already exist by username return false
        public bool AddImage(Pictures picture, byte[] image)
        {
            if (CheckImageExist(image))
            {
                return false;
            }
            else
            {
                newConnection.Insert(picture);
                return true;
            }
        }

        public bool DeleteImage(object picture)
        {
            if (newConnection.Delete(picture) != 0)
                return true;
            else return false;
        }

        /// <summary>
        /// delete all User in User Table
        /// </summary>
        /// <returns></returns>
        public int DeleteAllImages()
        {
            return newConnection.DeleteAll<Pictures>();
        }

        //obsolete
/*        public bool DeleteUserbyImagePath(string imagepath)
        {
            if (CheckImageExist(imagepath))
            {
                return false;
            }
            else
            {
                var data = newConnection.Table<Pictures>();
                var image = (from values in data
                            where values.ImagePath == imagepath
                            select values).Single();
                var succes = newConnection.Delete(image);
                if (succes == 1)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Image couldnt be deleted");
                    return false;
                }
            }
        }*/


        //------------------------------------------------------DB SEARCH METHODS-----------------------------------------------------------------------------------------------

        //check if user already exist in table by name
        public bool CheckImageExist(byte[] image)
        {
            var data = newConnection.Table<Pictures>();
            var d1 = data.Where(x => x.Image == image).FirstOrDefault();
            if (d1 != null)
            {
                return true;
            }
            else
                return false;
        }

        //------------------------------------------------------DB GET METHODS-----------------------------------------------------------------------------------------------
        //get first user on user table
        public Pictures GetFirstImage()
        {
            return newConnection.Table<Pictures>().First();
        }

        /// <summary>
        /// get list of images with specific type
        /// </summary>
        /// <returns>userobj</returns>
        public List<Pictures> GetImagesByType(string typename)
        {
            var data = newConnection.Table<Pictures>();
            var imagelist = new List<Pictures>();

            switch (typename)
            {
                case constants.typebad:
                    try
                    {
                        var imageList = (from values in data
                                         where values.Type == PicType.Bad
                                         select values).ToList();

                        return imageList;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                case constants.typegood:
                    try
                    {
                        var imageList = (from values in data
                                         where values.Type == PicType.Good
                                         select values).ToList();

                        return imageList;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                default:
                    Console.WriteLine("wrong pictype");
                    return null;
            }        
        }


        /// <summary>
        /// Get Image by ID
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public Pictures GetImageByID(int imageId)
        {
            var data = newConnection.Table<Pictures>();
            try
            {
                var user = (from values in data
                            where values.TypeId == imageId
                            select values).Single();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //get all Images as a List of Image Objects
        public List<Pictures> GetAllImagesToList()
        {
            var imageList = newConnection.Table<Pictures>().ToList();
            return imageList;
        }

        //get all Images as Collection
        public ObservableCollection<Pictures> GetAllUserToCollection()
        {
            var imageList = GetAllImagesToList();
            ObservableCollection<Pictures> ImageCollection = new ObservableCollection<Pictures>();

            foreach (var item in imageList)
            {
                ImageCollection.Add(item);
            }
            return ImageCollection;
        }


        //get all Images by Collection by inserting imageList
        public ObservableCollection<Pictures> GetAllUserByListToCollection(List<Pictures> images)
        {
            ObservableCollection<Pictures> ImageCollection = new ObservableCollection<Pictures>();
            foreach (var item in images)
            {
                ImageCollection.Add(item);
            }
            return ImageCollection;
        }

        // Get the  Image property by image
        public string getUserProperty(string property,byte[] image)
        {
            var data = newConnection.Table<Pictures>();
            try
            {
                var returnedImage = (from values in data
                                    where values.Image == image
                                     select values).Single();

                if (returnedImage != null)
                {
                    switch (property.Trim().ToLower())
                    {
                        case constants.imageid:
                            return returnedImage.TypeId.ToString();
                        case constants.imagetype:
                            return returnedImage.Type.ToString();
                        case constants.imagepath:
                            return returnedImage.ImagePath;
                        default:
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return string.Empty;
        }

        //------------------------------------------------------DEBUG METHODS-----------------------------------------------------------------------------------------------
        public void PrintImage(Pictures image)
        {
            var message =
                "ID " + image.TypeId.ToString() +
                "\nType " + image.Type.ToString() +
                "\nImagePath " + image.ImagePath;
            Console.WriteLine(message);
        }

        public void PrintAllImages()
        {
            var data = newConnection.Table<Pictures>();
            foreach (var image in data)
            {
                var message =
                "ID " + image.TypeId.ToString() +
                "\nType " + image.Type.ToString() +
                "\nImagePath " + image.ImagePath;
                Console.WriteLine(message);
            }
        }

        public async void debugImage(Pictures image)
        {
            var message = "ID " + image.TypeId.ToString() +
                          "\nType " + image.Type.ToString() +
                          "\nImagePath " + image.ImagePath;
            await App.Current.MainPage.DisplayAlert("Debug", message, "OK");

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
