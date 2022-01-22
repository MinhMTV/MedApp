using App1.Helpers;
using App1.Models;
using App1.PopUpViews;
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

namespace App1.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserCollectionPage : ContentPage
    {

        private UserCollectionViewModel _uvm;
        private UserDBHelper userDBHelper = new UserDBHelper();

        private bool isAdmin = true;
        public UserCollectionPage()
        {
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
                _uvm.IsAscending = true;
            });

            MessagingCenter.Subscribe<App, string>(App.Current, "PopUpSort", (snd, arg) =>
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
                _uvm.IsAscending = true;
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
            await Navigation.PushModalAsync(new AscendingPopUp());

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
                        userDBHelper.DeleteUser(item);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        uint duration = 100;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                Console.WriteLine($"Running: {e.TotalY}");
                if (e.TotalY > 0)
                {
                    BottomToolbar.TranslationY = openY + e.TotalY;
                }

            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                //Debug.WriteLine($"Completed: {e.TotalY}");
                if (lastPanY < 110)
                {
                    await OpenDrawer();
                }
                else
                {
                    await CloseDrawer();
                }
                isBackdropTapEnabled = true;
            }
        }

        async Task OpenDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(1, length: duration),
                BottomToolbar.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = false;
        }

        async Task CloseDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: duration),
                BottomToolbar.TranslateTo(0, 0, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = true;
        }

        private void UserCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asdfasdfasdf = UserCV.SelectedItems;
            
        }
    }
}