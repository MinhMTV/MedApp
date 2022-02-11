using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.PopUpViews;
using CBMTraining.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CBMTraining.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoGalleryPage : ContentPage
    {
        private PhotoCollectionViewModel _pvm;
        private PictureDBHelper picturesDBHelper = new PictureDBHelper();

        public PhotoGalleryPage()
        {
            InitializeComponent();
            BindingContext = _pvm = new PhotoCollectionViewModel();

            MessagingCenter.Subscribe<App, string>(App.Current, constants.imagePopup , (snd, arg) =>
            {
                _pvm.SelectionMode = SelectionMode.None;
                _pvm.SelectedPictures.Clear();
                _pvm.SelectedPictures_IsVisible = false;
                _pvm.Pictures.Clear();
                var user = picturesDBHelper.GetAllImagesToListByOrder(bool.Parse(arg));
                foreach (var item in user)
                {
                    _pvm.Pictures.Add(item);

                }
                _pvm.IsAscending = bool.Parse(arg);
            });
        }

        async void Sortby_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new AscendingPopUp(constants.imagePopup)); //set isImage to false, because we want User sort

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
