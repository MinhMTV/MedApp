using CBMTraining.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CBMTraining.Helpers;

using Xamarin.Forms;
using CBMTraining.PopUpViews;
using Xamarin.Essentials;
using CBMTraining.View.AdminPages;

namespace CBMTraining.ViewModels
{
    public class AllBadPhotosCollectionViewModel : BaseViewModel
    {
        private ObservableCollection<Pictures> _pictures = new ObservableCollection<Pictures>();

        public ObservableCollection<Pictures> Pictures { get => _pictures; set => _pictures = value; }

        public ObservableCollection<object> _selectedpictures { get; set; }

        private int _nrOfSelectedPictures { get; set; }

        private bool _selectedPictures_IsVisible;

        private SelectionMode _selectionMode = SelectionMode.None;

        public ObservableCollection<object> SelectedPictures { get => _selectedpictures; set => _selectedpictures = value; }

        public SelectionMode SelectionMode { get => _selectionMode; set => SetProperty(ref _selectionMode, value); }

        public bool IsAscending { get; set; } = false; //Set to Default false for order newest to oldest Picture

        public Command<Pictures> LongPressCommand { get; private set; }
        public Command OrderByCommand { get; set; }
        public Command ClearCommand { get; private set; }
        public Command RefreshCommand { get; }
        public Command<Pictures> PressedCommand { get; private set; }

        public PictureDBHelper picturesDbHelper = new PictureDBHelper();


        public AllBadPhotosCollectionViewModel()
        {
            InitData();

            LongPressCommand = new Command<Pictures>(OnLongPress);
            ClearCommand = new Command(OnClear);
            PressedCommand = new Command<Pictures>(OnPressed);
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }

        private void InitData()
        {
            _selectedpictures = new ObservableCollection<object>();
            _pictures = picturesDbHelper.GetAllBadImagesToCollectionByOrder(Settings.isBadImageAscending);
            _nrOfSelectedPictures = SelectedPictures.Count();
            _selectedPictures_IsVisible = false;
        }

        public int NrofSelectedPictures
        {
            get => _nrOfSelectedPictures;
            set
            {
                _nrOfSelectedPictures = value;
                OnPropertyChanged(nameof(NrofSelectedPictures));
            }
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

        public bool SelectedPictures_IsVisible
        {
            get { return _selectedPictures_IsVisible; }
            set
            {
                _selectedPictures_IsVisible = value;
                OnPropertyChanged(nameof(SelectedPictures_IsVisible));
            }
        }

        private void ExecuteRefreshCommand()
        {
            _pictures.Clear();
            _selectedpictures.Clear();
            IsRefreshing = true;

            foreach (var item in picturesDbHelper.GetAllImagesToListByOrder(Settings.isBadImageAscending))
            {
                _pictures.Add(item);
            }
            SelectedPictures_IsVisible = false;
            NrofSelectedPictures = SelectedPictures.Count();
            SelectionMode = SelectionMode.None;
            // Stop refreshing
            IsRefreshing = false;
        }

        private async void OnPressed(Pictures obj)
        {
            if (_selectionMode != SelectionMode.None)
            {
                Console.WriteLine("Test OnPress");
                if (_selectedpictures.Count() == 0)
                {
                    SelectionMode = SelectionMode.None;
                    SelectedPictures_IsVisible = false;
                }
                else
                {
                    SelectedPictures_IsVisible = true;
                    Console.WriteLine("Test OnPress");
                }

            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new PhotoDetails(obj.Image), true);
            }

            NrofSelectedPictures = SelectedPictures.Count();
        }

        private void OnClear()
        {
            SelectionMode = SelectionMode.None;
        }

        private void OnLongPress(Pictures p)
        {
            try
            {
                // Use default vibration length
                Vibration.Vibrate();
                if (_selectionMode == SelectionMode.None)
                {
                    SelectionMode = SelectionMode.Multiple;
                    SelectedPictures.Add(p);
                    NrofSelectedPictures++;
                    SelectedPictures_IsVisible = true;
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine("Feature not supported on device" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            NrofSelectedPictures = SelectedPictures.Count();
        }
    }

}
