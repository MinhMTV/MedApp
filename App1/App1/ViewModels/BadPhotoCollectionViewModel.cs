using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using App1.Helpers;

using Xamarin.Forms;
using App1.PopUpViews;
using Rg.Plugins.Popup;

namespace App1.ViewModels
{
    public class BadPhotoCollectionViewModel : BaseViewModel
    {
        private ObservableCollection<Pictures> _pictures = new ObservableCollection<Pictures>();

        public ObservableCollection<Pictures> Pictures { get => _pictures; set => _pictures = value; }

        public Command RefreshCommand { get; }
        public Command<Pictures> PressedCommand { get; private set; }

        public Command<Pictures> LongPressCommand { get; private set; }

        public PictureDBHelper picturesDbHelper = new PictureDBHelper();


        public BadPhotoCollectionViewModel()
        {
            InitData();
            LongPressCommand = new Command<Pictures>(OnLongPress);
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }

        private void InitData()
        {
            _pictures = picturesDbHelper.GetAllBadImagesToCollection();
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                SetProperty(ref isRefreshing, value);
            }
        }

        private void ExecuteRefreshCommand()
        {

        }

        private async void OnLongPress(Pictures obj)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new PhotoPopUp(obj.Image), true);
        }




    }
}