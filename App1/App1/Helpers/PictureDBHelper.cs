using CBMTraining.Database;
using CBMTraining.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Reflection;
using CBMTraining.DependencyServices;
using CBMTraining.Methods;

namespace CBMTraining.Helpers
{
    public class PictureDBHelper
    {
        private SQLiteConnection newConnection;
        DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();


        public PictureDBHelper()
        {
            newConnection = DependencyService.Get<ISQLite>().GetConnection();
            newConnection.CreateTable<Pictures>(); // Create table if not exists
            //           AddPictures();
        }

        //------------------------------------------------------DB Init METHODS-----------------------------------------------------------------------------------------------

        //initialize the embedded pictures
        public async void initGivenPictures()
        {
            var ImageResizer = DependencyService.Get<IImageResizer>();
            var ImageResizerWin = DependencyService.Get<IImageResizerWin>();


            var screenwidth = deviceMetricHelper.getWidth();
            var screenheight = deviceMetricHelper.getHeight();

            screenwidth = screenwidth * 0.4;
            screenheight = screenheight * 0.4;


            for (int i = 1; i <= GlobalVariables.NroOfAvailablePics; i++)
            {
                byte[] newImage;
                Console.WriteLine("counter i: " + i.ToString());
                if (i <= GlobalVariables.NrOfAvailableGoodPics)
                {
                    var imageArray = ImageDataFromResource(constants.ImageFolderPath + "g" + i.ToString("00") + ".jpg");
                    if (imageArray != null)
                    {

                        if (Device.RuntimePlatform == Device.UWP)
                        {
                            newImage = await ImageResizerWin.ResizeImageWindows(imageArray, (float)screenwidth, (float)screenheight);
                        }
                        else
                        {
                            newImage = ImageResizer.ResizeImage(imageArray, (float)screenwidth, (float)screenheight);
                        }

                        Pictures picture = new Pictures { TypeId = i, Type = PicType.Good, Image = newImage, Photo = "g" + i.ToString("00") + ".jpg" };
                        if (!CheckImageExist(imageArray))
                        {

                            newConnection.Insert(picture);
                        }
                        else
                        {
                            Console.WriteLine("Image already exist");
                        }
                    }
                }
                else
                {
                    var imageArray = ImageDataFromResource(constants.ImageFolderPath + "b" + (i - GlobalVariables.NrOfAvailableGoodPics).ToString("00") + ".jpg");
                    if (imageArray != null)
                    {
                        if (Device.RuntimePlatform == Device.UWP)
                        {
                            newImage = await ImageResizerWin.ResizeImageWindows(imageArray, (float)screenwidth, (float)screenheight);
                        }
                        else
                        {
                            newImage = ImageResizer.ResizeImage(imageArray, (float)screenwidth, (float)screenheight);
                        }

                        Pictures picture = new Pictures { TypeId = i, Type = PicType.Bad, Image = imageArray, Photo = "b" + i.ToString("00") + ".jpg" };
                        if (!CheckImageExist(imageArray))
                        {
                            newConnection.Insert(picture);
                        }
                        else
                        {
                            Console.WriteLine("Image already exist");
                        }
                    }
                }  
            }
        }


        //convert Image fom Resource to byte[]
        public byte[] ImageDataFromResource(string r)
        {
            // Ensure "this" is an object that is part of your implementation within your Xamarin forms project
            var assembly = this.GetType().GetTypeInfo().Assembly;
            byte[] buffer = null;

            using (System.IO.Stream s = assembly.GetManifestResourceStream(r))
            {
                if (s != null)
                {
                    long length = s.Length;
                    buffer = new byte[length];
                    s.Read(buffer, 0, (int)length);
                }
            }

            return buffer;
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

        //if a table gets delete, delete also the autoincrementID to force new table starting by one
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
        public ObservableCollection<Pictures> GetAllImagesToCollection()
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
        public ObservableCollection<Pictures> GetAllImagesByListToCollection(List<Pictures> images)
        {
            ObservableCollection<Pictures> ImageCollection = new ObservableCollection<Pictures>();
            foreach (var item in images)
            {
                ImageCollection.Add(item);
            }
            return ImageCollection;
        }

        public List<Pictures> GetAllGoodImagesToList()
        {
            var imageList = newConnection.Table<Pictures>().Where(x => x.Type == PicType.Good).ToList();
            return imageList;
        }

        public ObservableCollection<Pictures> GetAllGoodImagesToCollection()
        {
            var imagelist = GetAllGoodImagesToList();
            return GetAllImagesByListToCollection(imagelist);
        }

        public List<Pictures> GetAllBadImagesToList()
        {
            var imageList = newConnection.Table<Pictures>().Where(x => x.Type == PicType.Bad).ToList();
            return imageList;
        }

        public ObservableCollection<Pictures> GetAllBadImagesToCollection()
        {
            var imagelist = GetAllBadImagesToList();
            return GetAllImagesByListToCollection(imagelist);
        }


        /// <summary>
        /// get all Pictures By Collection by specific order
        /// </summary>
        /// <param name="isAscending">true = sort order Ascending, false = Descending</param>
        /// <returns></returns>
        public ObservableCollection<Pictures> GetAllImagesByOrder(bool isAscending)
        {
            List<Pictures> imageList = GetAllImagesToListByOrder(isAscending);
            return GetAllImagesByListToCollection(imageList);
        }

        public List<Pictures> GetAllImagesToListByOrder(bool isAscending)
        {
            var data = newConnection.Table<Pictures>();

            if (isAscending)
            {
                return data.OrderBy(x => x.TypeId).ToList();
            }
            else
            {
                return data.OrderByDescending(x => x.TypeId).ToList();
            }
        }

        public List<Pictures> GetAllGoodImagesToListByOrder(bool isAscending)
        {
            if (isAscending)
            {
                return newConnection.Table<Pictures>().Where(x => x.Type == PicType.Good).OrderBy(x => x.TypeId).ToList();
            }
            else
            {
                return newConnection.Table<Pictures>().Where(x => x.Type == PicType.Good).OrderByDescending(x => x.TypeId).ToList();
            }
        }

        public ObservableCollection<Pictures> GetAllGoodImagesToCollectionByOrder(bool isAscending)
        {
            var imagelist = GetAllGoodImagesToListByOrder(isAscending);
            return GetAllImagesByListToCollection(imagelist);
        }

        public List<Pictures> GetAllBadImagesToListByOrder(bool isAscending)
        {
            if (isAscending)
            {
                return newConnection.Table<Pictures>().Where(x => x.Type == PicType.Bad).OrderBy(x => x.TypeId).ToList();
            }
            else
            {
                return newConnection.Table<Pictures>().Where(x => x.Type == PicType.Bad).OrderByDescending(x => x.TypeId).ToList();
            }
        }

        public ObservableCollection<Pictures> GetAllBadImagesToCollectionByOrder(bool isAscending)
        {
            var imagelist = GetAllBadImagesToListByOrder(isAscending);
            return GetAllImagesByListToCollection(imagelist);
        }


        // Get the  Image property by image
        public string getImageProperty(string property,byte[] image)
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
    }
}
