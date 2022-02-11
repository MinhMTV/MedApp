using CBMTraining.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CBMTraining.Helpers;

using Xamarin.Forms;
using CBMTraining.PopUpViews;
using Rg.Plugins.Popup;

namespace CBMTraining.ViewModels
{
    public class GoodPhotoCollectionViewModel : BaseViewModel
    {
        private ObservableCollection<Pictures> _pictures = new ObservableCollection<Pictures>();

        public ObservableCollection<Pictures> Pictures { get => _pictures; set => _pictures = value; }
 
        public Command RefreshCommand { get; }
        public Command<Pictures> PressedCommand { get; private set; }

        public Command<Pictures> LongPressCommand { get; private set; }

        public PictureDBHelper picturesDbHelper = new PictureDBHelper();


        public GoodPhotoCollectionViewModel()
        {
            InitData();
            LongPressCommand = new Command<Pictures>(OnLongPress);
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }

        private void InitData()
        {
            _pictures = picturesDbHelper.GetAllGoodImagesToCollection();
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