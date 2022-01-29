using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Helpers;
using App1.Models;
using App1.PopUpViews;
using App1.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodPhotoGalleryPage : ContentPage
    {
        private AllGoodPhotosCollectionView _pvm;
        private PictureDBHelper picturesDBHelper = new PictureDBHelper();

        public GoodPhotoGalleryPage()
        {
            InitializeComponent();
            BindingContext = _pvm = new AllGoodPhotosCollectionView();

            MessagingCenter.Subscribe<App, string>(App.Current, constants.goodimagePopup, (snd, arg) =>
            {
                _pvm.SelectionMode = SelectionMode.None;
                _pvm.SelectedPictures.Clear();
                _pvm.SelectedPictures_IsVisible = false;
                _pvm.Pictures.Clear();
                var user = picturesDBHelper.GetAllGoodImagesToListByOrder(bool.Parse(arg));
                foreach (var item in user)
                {
                    _pvm.Pictures.Add(item);

                }
                _pvm.IsAscending = bool.Parse(arg);
            });
        }

        async void Sortby_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new AscendingPopUp(constants.goodimagePopup)); //set isImage to false, because we want User sort

        }
        async void OnTrashTapped(object sender, EventArgs args)
        {
            var result = await this.DisplayAlert("Achtung!", "Wollen Sie die Bilder wirklich löschen?", "Ja", "Nein");

            if (result)
            {
                try
                {
                    var userlist = PicCV.SelectedItems.ToList();
                    foreach (var item in userlist)
                    {
                        picturesDBHelper.DeleteImage(item);
                        _pvm.Pictures.Remove((Pictures)item);
                    }
                    _pvm.SelectionMode = SelectionMode.None;
                    _pvm.SelectedPictures.Clear();
                    _pvm.SelectedPictures_IsVisible = false;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //do nothing
            }

        }

        void ClearSelect_Clicked(System.Object sender, System.EventArgs e)
        {
            //           await CloseDrawer();
            _pvm.SelectionMode = SelectionMode.None;
            _pvm.SelectedPictures.Clear();
            _pvm.SelectedPictures_IsVisible = false;
        }
    }
}