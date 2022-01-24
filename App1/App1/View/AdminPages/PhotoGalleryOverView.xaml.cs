using App1.Methods;
using NativeMedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.DependencyServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoGalleryOverView : ContentPage
    {
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

                    if (ImageResizer != null)
                    {
                        var newImage = ImageResizer.ResizeImage(imagearray, 1000,1000);
                        Console.WriteLine(newImage);

                        testimage.Source = ImageSource.FromStream(() =>
                        {
                            return new MemoryStream(newImage);
                        });

                    }

                    

                    await DisplayAlert("test", imageSource.ToString(), "OK");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}