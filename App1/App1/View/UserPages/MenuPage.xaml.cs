using App1.Helpers;
using App1.Models;
using App1.View.GeneralPages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : FlyoutPage
    {

        public MenuPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new DemoPage());
            IsPresented = false;
            var loguser = Preferences.Get(constants.loginUser, "false");
            name.Text = "Hallo " + loguser;
            Console.WriteLine(Preferences.Get(constants.loginUser, "false"));
        }

        // This prevents a user from being able to hit the back button and leave the Menu Page.
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsPresented = false;
        }

        void Demo_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new DemoPage());
            IsPresented = false;
        }

        void WeeklyOverviewClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new WeeklyOverviewPage());
            IsPresented = false;
            
        }

        void EditUserInformationClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new EditUserInformtionPage());
            IsPresented = false;
        }

        async void LogOutClicked(object sender, System.EventArgs e)
        {
            var decision = await DisplayAlert("Achtung", "Möchten Sie sich wirklich ausloggen?", "Ja", "Nein");
            if (decision)
            {
                UserDBHelper userDBHelper = new UserDBHelper();
                userDBHelper.LogOutUser();
                Preferences.Set(constants.loginUser, "false");
                await Navigation.PushAsync(new LoginPage());
            }
            IsPresented = false;
        }

    }
}