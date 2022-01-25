using App1.Methods;
using NativeMedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.DependencyServices;
using App1.Extensions;
using App1.Helpers;
using App1.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoGalleryOverView : ContentPage
    {
        DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();
        PictureDBHelper picturesDBHelper = new PictureDBHelper();

        public PhotoGalleryOverView()
        {
            InitializeComponent();
            
        }
        async void OnAddImageGoodTapped1(object sender, EventArgs args)
        {
            try
            {
                var results = await MediaGallery.PickAsync(3, MediaFileType.Image);



                if (results.Files == null)
                {
                    return;
                }
                foreach (var media in results.Files)
                {
                    var fileName = media.NameWithoutExtension;
                    var extension = media.Extension;
                    var contenType = media.ContentType;
                    var imagepath = media.ToString();
                    var stream  = media.OpenReadAsync();

                    await DisplayAlert(fileName, $"Extension: {extension} Content-type: {contenType}, imagepath:{imagepath}, stream: {stream}", "OK");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void OnAddImageGoodTapped(object sender, EventArgs args)
        {

            
            try
            {
                var results = await MediaGallery.PickAsync(3, MediaFileType.Image);

                if (results.Files == null)
                {
                    return;
                }

                foreach (var media in results.Files)
                {
                    var imagestream = await media.OpenReadAsync();
                    var imageSource = ImageSource.FromStream((() => imagestream));

                    //               testimage.Source = imageSource;
                    var imagearray = ImageHelper.ConvertStreamToByteArray(imagestream);
                    Console.WriteLine(imagearray);

                    var ImageResizer = DependencyService.Get<IImageResizer>();

                    var screenwidth = deviceMetricHelper.getWidth();
                    var screenheight = deviceMetricHelper.getHeight();

                    screenwidth = screenwidth * 0.8;
                    screenheight = screenheight * 0.8;

                    if (ImageResizer != null)
                    {
                        var newImage = ImageResizer.ResizeImage(imagearray, (float) screenwidth, (float) screenheight);
                        Console.WriteLine(newImage);
                        Pictures pic = new Pictures();
                        pic.Type = PicType.Good;
                        pic.Image = newImage;
                        picturesDBHelper.AddImage(pic, newImage);
                        //code if we safe the image in local storage
                        /*                        try
                                                {
                                                    var rootfolder = FileExtensions.getFolderPathShare();
                                                    Console.WriteLine("RootFolder: " + rootfolder);
                                                    var filepath = await FileExtensions.SaveToLocalFolderAsync(stream,"test2.jpg", rootfolder);
                                                    Console.WriteLine("FilePath: " + filepath);
                        //                            await DisplayAlert("Test", $"filepath: {filepath}", "OK");

                                                    Stream stream2 = await FileExtensions.LoadFileStreamAsync(filepath);

                                                    Console.WriteLine(stream2.ToString());

                                                    testimage.Source = Path.Combine(rootfolder,"test2.jpg");
                                                    Console.WriteLine(Path.Combine(rootfolder,"test2.jpg"));

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.ToString());
                                                }
                        var folderpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                                            Console.WriteLine(folderpath.ToString());
                        */

                    }





                    await DisplayAlert("Test", $"screenwidth: {screenwidth} , screenheigth {screenheight}" , "OK");
                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            var imagebyte = picturesDBHelper.GetFirstImage();

            testimage.Source = ImageSource.FromStream(() =>
            {
                return new MemoryStream(imagebyte.Image);
            });
        }
    }
}