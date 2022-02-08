using App1.Helpers;
using App1.Models;
using App1.View.GeneralPages;
using App1.View.UserPages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminMenu : FlyoutPage
    {
        public AdminMenu()
        { 
            InitializeComponent();
            Detail = new NavigationPage(new UserCollectionPage());
            IsPresented = false;
            var loguser = Preferences.Get(constants.loginUser, "false");
            name.Text = "Hallo Therapeut " + loguser;
            Console.WriteLine(Preferences.Get(constants.loginUser, "false"));
        }

        // This prevents a user from being able to hit the back button and leave the Admin Page.
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsPresented = false;
        }

        void ListUserView_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new UserCollectionPage());
            IsPresented = false;
            GlobalVariables.isGallery = false;

        }

        async void PhotoGalleryClicked(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await DisplayAlert("Achtung", "Diese Funktion wird derzeit nicht unterstützt", "OK");
                return;
            }
            Detail = new NavigationPage(new PhotoGalleryOverView());
            IsPresented = false;
            GlobalVariables.isGallery = true;
        }

        void EditUserInformationClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new EditUserInformtionPage());
            IsPresented = false;
            GlobalVariables.isGallery = false;
        }

        void SettingClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new SettingsPage());
            IsPresented = false;
            GlobalVariables.isGallery = false;
        }



        async void LogOutClicked(object sender, System.EventArgs e)
        {
            var decision = await DisplayAlert("Achtung", "Möchten Sie sich wirklich ausloggen?", "Ja", "Nein");
            if (decision)
            {
                AdminDBHelper adminDBHelper = new AdminDBHelper();
                adminDBHelper.LogOutUser();
                await Navigation.PushAsync(new LoginPage());
            }
            IsPresented = false;
            GlobalVariables.isGallery = false;
        }
    }
}