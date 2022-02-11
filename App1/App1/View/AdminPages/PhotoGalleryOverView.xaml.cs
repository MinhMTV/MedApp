using CBMTraining.Methods;
using NativeMedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBMTraining.DependencyServices;
using CBMTraining.Extensions;
using CBMTraining.Helpers;
using CBMTraining.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CBMTraining.ViewModels;


namespace CBMTraining.View.AdminPages
{
    
    public partial class PhotoGalleryOverView : ContentPage
    {
        DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();
        PictureDBHelper picturesDBHelper = new PictureDBHelper();
        

        public PhotoGalleryOverView()
        {
            InitializeComponent();
            GlobalVariables.isGallery = true;
        }

        async void AllGoodTapped(object sender, EventArgs e)
        {

            await Application.Current.MainPage.Navigation.PushAsync(new GoodPhotoGalleryPage());
        }

        async void AllBadTapped(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new BadPhotoGalleryPage());
        }

        async void ShowAll_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PhotoGalleryPage());
        }

        async void OnAddImageGoodTapped(object sender, EventArgs args)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await DisplayAlert("Achtung", "Diese Funktion wird noch nicht unterstützt", "ok");
                return;
            }

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

                    var imagearray = ImageHelper.ConvertStreamToByteArray(imagestream);
                    Console.WriteLine(imagearray);

                    var ImageResizer = DependencyService.Get<IImageResizer>();

                    var screenwidth = deviceMetricHelper.getWidth();
                    var screenheight = deviceMetricHelper.getHeight();

                    screenwidth = screenwidth * 0.5;
                    screenheight = screenheight * 0.5;

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
                    // Debug await DisplayAlert("Test", $"screenwidth: {screenwidth} , screenheigth {screenheight}" , "OK");
                    
                }

            }
            catch (Exception ex)
            {
                    throw ex;
                   
            }
        }
        async void OnAddImageBadTapped(object sender, EventArgs args)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await DisplayAlert("Achtung", "Diese Funktion wird noch nicht unterstützt", "ok");
                return;
            }

            try
            {
                GlobalVariables.isGallery = true;
                var results = await MediaGallery.PickAsync(3, MediaFileType.Image);

                if (results.Files == null)
                {
                    return;
                }

                foreach (var media in results.Files)
                {
                    var imagestream = await media.OpenReadAsync();
                    var imageSource = ImageSource.FromStream((() => imagestream));

                    var imagearray = ImageHelper.ConvertStreamToByteArray(imagestream);
                    Console.WriteLine(imagearray);

                    var ImageResizer = DependencyService.Get<IImageResizer>();

                    var screenwidth = deviceMetricHelper.getWidth();
                    var screenheight = deviceMetricHelper.getHeight();

                    screenwidth = screenwidth * 0.5;
                    screenheight = screenheight * 0.5;

                    if (ImageResizer != null)
                    {
                        var newImage = ImageResizer.ResizeImage(imagearray, (float)screenwidth, (float)screenheight);
                        Console.WriteLine(newImage);
                        Pictures pic = new Pictures();
                        pic.Type = PicType.Bad;
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
                    // Debug await DisplayAlert("Test", $"screenwidth: {screenwidth} , screenheigth {screenheight}" , "OK");

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}