using App1.Helpers;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;


namespace App1.ViewModels
{
    public class UserCollectionViewModel : BaseViewModel
    {
        private ObservableCollection<User> _user;

        public ObservableCollection<object> _selectedUser { get; set; }

        private SelectionMode _selectionMode = SelectionMode.None;

        private int _nrOfSelectedUser { get; set; }

        public User SelectedItem { get; set; }

        public SelectionMode SelectionMode { get => _selectionMode; set => SetProperty(ref _selectionMode, value); }

        public ObservableCollection<User> User { get => _user; set => _user = value; }

        public ObservableCollection<object> SelectedUser { get => _selectedUser; set => _selectedUser = value; }

        public Command ShareCommand { get; set; }

        public Command<User> LongPressCommand { get; private set; }

        public Command ClearCommand { get; private set; }
        public Command RefreshCommand { get; }
        public Command<User> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        public UserCollectionViewModel()
        {
            InitData();
            
            ShareCommand = new Command(OnShare);
            LongPressCommand = new Command<User>(OnLongPress);
            ClearCommand = new Command(OnClear);
            PressedCommand = new Command<User>(OnPressed);
            RefreshCommand = new Command(ExecuteRefreshCommand);
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

        public int NrofSelectedUser
        {
            get => _nrOfSelectedUser;
            set
            {
                _nrOfSelectedUser = value;
                RaisePropertyChanged(nameof(NrofSelectedUser));
            }
        }

        private async void ExecuteRefreshCommand()
        {
            _user.Clear();
            IsRefreshing = true;
            foreach(var item in userDBHelper.GetAllUserToList())
            {
                _user.Add(item);
            }         

            // Stop refreshing
            IsRefreshing = false;
        }

        private async void OnPressed(User obj)
         {
            Console.WriteLine("User In Collection: " + SelectedUser.Count());
            if (_selectionMode != SelectionMode.None)
            {
                _nrOfSelectedUser = SelectedUser.Count();
                Console.WriteLine("Test OnPress");
                if (_selectedUser.Count() == 0)
                {
                    SelectionMode = SelectionMode.None;
                }
                else
                {
                    Console.WriteLine("Test OnPress");
                }
                
            }
            else
            {
                await  App.Current.MainPage.DisplayToastAsync("Navigiere zu User Details");
            }
        }

        private void OnClear()
        {
            SelectionMode = SelectionMode.None;
        }

        private void OnLongPress(User p)
        {
            Console.WriteLine("LongPressed");
            if (_selectionMode == SelectionMode.None)
            {
                SelectionMode = SelectionMode.Multiple;
                SelectedUser.Add(p);
                _nrOfSelectedUser = SelectedUser.Count();
                Console.Write("Add User");
            }
        }

        private async void OnShare()
        {
            
        }



        private void InitData()
        {
            _selectedUser = new ObservableCollection<object>();
            _user = userDBHelper.GetAllUserToCollection();
            _nrOfSelectedUser = SelectedUser.Count();
            RaisePropertyChanged(nameof(User));
        }
    }

}