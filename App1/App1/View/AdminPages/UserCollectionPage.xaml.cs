﻿using App1.Helpers;
using App1.Models;
using App1.PopUpViews;
using App1.View.GeneralPages;
using App1.ViewModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserCollectionPage : ContentPage
    {

        private UserCollectionViewModel _uvm;
        private UserDBHelper userDBHelper = new UserDBHelper();
        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private bool isAdmin = true;
        public UserCollectionPage()
        {
            var userList = userDBHelper.GetAllUserToList();
            foreach (var user in userList)
            {
                if (trainingSession.getLastTrainingSessionbyUser(user) != null)
                {
                    user.LastSession = trainingSession.getLastTrainingSessionbyUser(user).SessionDate;
                    userDBHelper.UpdateUser(user);
                }
                else
                {
                    user.LastSession = DateTime.MaxValue; // assigns default value 01/01/0001 00:00:00
                    userDBHelper.UpdateUser(user);
                }

                if (trainingSession.getFirstTrainingSessionbyUser(user) != null)
                {
                    user.FirstSession = trainingSession.getFirstTrainingSessionbyUser(user).SessionDate;
                    userDBHelper.UpdateUser(user);
                }
                else
                {
                    user.FirstSession = DateTime.MinValue; // assigns default value 31/12/9999  11:59:59
                    userDBHelper.UpdateUser(user);
                }
            }
            

            BindingContext = _uvm = new UserCollectionViewModel();
            InitializeComponent();
            

            MessagingCenter.Subscribe<App, string>(App.Current, "PopUpOrder", (snd, arg) =>
            {
                _uvm.SelectionMode = SelectionMode.None;
                _uvm.SelectedUser.Clear();
                _uvm.SelectedUser_IsVisible = false;
                _uvm.User.Clear();
                var user = userDBHelper.GetAllUserToListByOrder(arg.ToString(), bool.Parse(Preferences.Get(constants.isAscending,"true")));
                foreach(var item in user)
                {
                    _uvm.User.Add(item);

                }
               _uvm.UserOrderBy = arg.ToString();
                _uvm.IsAscending = bool.Parse(Preferences.Get(constants.isAscending, "true"));
            });

            MessagingCenter.Subscribe<App, string>(App.Current, constants.userPopup, (snd, arg) =>
            {
                _uvm.SelectionMode = SelectionMode.None;
                _uvm.SelectedUser.Clear();
                _uvm.SelectedUser_IsVisible = false;
                _uvm.User.Clear();
                var user = userDBHelper.GetAllUserToListByOrder(Preferences.Get(constants.OrderBy, "createdat"), bool.Parse(arg));
                foreach (var item in user)
                {
                    _uvm.User.Add(item);

                }
                _uvm.UserOrderBy = arg.ToString();
                _uvm.IsAscending = bool.Parse(Preferences.Get(constants.isAscending, "true"));
            });


        }
            async void AddUser_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new Registration(isAdmin), false);
             
        }

        async void OrderBy_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new OrderUserPopUp());

        }

        async void Sortby_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new AscendingPopUp(constants.userPopup)); //set isImage to false, because we want User sort

        }

        async void OnTrashTapped(object sender, EventArgs args)
        {
            var result = await this.DisplayAlert("Achtung!", "Wollen Sie die User wirklich löschen?", "Ja", "Nein");

            if (result)
            {
                try
                {
                    var userlist = UserCV.SelectedItems.ToList();
                    foreach (var item in userlist)
                    {
                        userDBHelper.DeleteUser((User)item);
                        _uvm.User.Remove((User)item);
                    }
                    _uvm.SelectionMode = SelectionMode.None;
                    _uvm.SelectedUser.Clear();
                    _uvm.SelectedUser_IsVisible = false;

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
            _uvm.SelectionMode = SelectionMode.None;
            _uvm.SelectedUser.Clear();
            _uvm.SelectedUser_IsVisible = false;
        }
    }
}