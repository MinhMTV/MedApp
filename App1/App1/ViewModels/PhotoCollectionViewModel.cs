using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class PhotoCollectionViewModel : BaseViewModel
    {
        private ObservableCollection<Pictures> _pictures = new ObservableCollection<Pictures>();

        public ObservableCollection<Pictures> Pictures { get => _pictures; set => _pictures = value; }

        public ObservableCollection<object> _selectedpictures{ get; set; }

        private int _nrOfSelectedPictures { get; set; }

        private bool _selectedPictures_IsVisible;

        private SelectionMode _selectionMode = SelectionMode.None;

        public ObservableCollection<object> SelectedPictures { get => _selectedpictures; set => _selectedpictures = value; }

        public SelectionMode SelectionMode { get => _selectionMode; set => SetProperty(ref _selectionMode, value); }

        public Command<User> LongPressCommand { get; private set; }
        public Command OrderByCommand { get; set; }
        public Command ClearCommand { get; private set; }
        public Command RefreshCommand { get; }
        public Command<User> PressedCommand { get; private set; }

        public PhotoCollectionViewModel()
        {
            InitData();

            LongPressCommand = new Command<User>(OnLongPress);
            ClearCommand = new Command(OnClear);
            PressedCommand = new Command<User>(OnPressed);
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }

        private void InitData()
        {
            _selectedpictures = new ObservableCollection<object>();
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
            
        }

        private async void OnPressed(User obj)
        {
            
        }

        private void OnClear()
        {
            SelectionMode = SelectionMode.None;
        }

        private void OnLongPress(User p)
        {
            
        }


    }
}