using App1.Helpers;
using App1.Models;
using App1.View.AdminPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace App1.ViewModels
{
    public class UserCollectionViewModel : BaseViewModel
    {
        private ObservableCollection<User> _user;

        public ObservableCollection<object> _selectedUser { get; set; }

        private SelectionMode _selectionMode = SelectionMode.None;

        private int _nrOfSelectedUser { get; set; }


        private bool _selectedUser_IsVisible;

        public SelectionMode SelectionMode { get => _selectionMode; set => SetProperty(ref _selectionMode, value); }

        public ObservableCollection<User> User { get => _user; set => _user = value; }

        public ObservableCollection<object> SelectedUser { get => _selectedUser; set => _selectedUser = value; }

        public String UserOrderBy { get; set; }

        public bool IsAscending { get; set; } = true;

        

        public Command<User> LongPressCommand { get; private set; }
        
        public Command OrderByCommand { get; set; }
        public Command ClearCommand { get; private set; }
        public Command RefreshCommand { get; }
        public Command<User> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        public UserCollectionViewModel()
        {
            InitData();
            
            LongPressCommand = new Command<User>(OnLongPress);
            ClearCommand = new Command(OnClear);
            PressedCommand = new Command<User>(OnPressed);
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }
        public int NrofSelectedUser
        {
            get => _nrOfSelectedUser;
            set
            {
                _nrOfSelectedUser = value;
                OnPropertyChanged(nameof(NrofSelectedUser));
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

        public bool SelectedUser_IsVisible
        {
            get { return _selectedUser_IsVisible; }
            set
            {
                _selectedUser_IsVisible = value;
                OnPropertyChanged(nameof(SelectedUser_IsVisible));
            }
        }

        private void ExecuteRefreshCommand()
        {
            _user.Clear();
            _selectedUser.Clear();
            IsRefreshing = true;

            foreach (var item in userDBHelper.GetAllUserToListByOrder(Settings.OrderBy,Settings.isAscending))
            {
                _user.Add(item);
            }
            SelectedUser_IsVisible = false;
            NrofSelectedUser = SelectedUser.Count();
            SelectionMode = SelectionMode.None; 
            // Stop refreshing
            IsRefreshing = false;
        }

        private async void OnPressed(User obj)
         {
            Console.WriteLine("User In Collection: " + SelectedUser.Count());
            if (_selectionMode != SelectionMode.None)
            {
                Console.WriteLine("Test OnPress");
                if (_selectedUser.Count() == 0)
                {
                    SelectionMode = SelectionMode.None;
                    SelectedUser_IsVisible = false;
                }
                else
                {
                    SelectedUser_IsVisible = true;
                    Console.WriteLine("Test OnPress");
                }
                
            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new UserDetailPage(obj));
            }

            NrofSelectedUser = SelectedUser.Count();
        }

        private void OnClear()
        {
            SelectionMode = SelectionMode.None;
        }

        private void OnLongPress(User p)
        {
            try
            {
                // Use default vibration length
                Vibration.Vibrate();
                Console.WriteLine("LongPressed");
                if (_selectionMode == SelectionMode.None)
                {
                    SelectionMode = SelectionMode.Multiple;
                    SelectedUser.Add(p);
                    NrofSelectedUser++;
                    SelectedUser_IsVisible = true;
                    Console.Write("Add User");
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

            NrofSelectedUser = SelectedUser.Count();
        }
        private void InitData()
        {
            _selectedUser = new ObservableCollection<object>();
            var userlist = userDBHelper.GetAllUserToListByOrder(Settings.OrderBy, Settings.isAscending);
            _user = userDBHelper.GetAllUserByListToCollection(userlist);
            _nrOfSelectedUser = SelectedUser.Count();
            _selectedUser_IsVisible = false;
        }
    }
}