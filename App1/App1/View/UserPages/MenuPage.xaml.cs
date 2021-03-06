using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.View.GeneralPages;
using CBMTraining.ViewModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CBMTraining.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : FlyoutPage
    {
        UserDBHelper userDBHelper = new UserDBHelper();
        private string loguser { get; set; }

        public string Loguser
        {
            get { return loguser; }
            set { loguser = value; 
            OnPropertyChanged(nameof(Loguser)); }

        }
        public MenuPage()
        {

            InitializeComponent();
            this.BindingContext = this;
            Detail = new NavigationPage(new ExplainPage());
            IsPresented = false;
            Loguser = Settings.loginUser;
            

            MessagingCenter.Subscribe<object, string>(this, "loguser", (sender, user) =>
            {
                Loguser = user;
            });
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

        void startTraining_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new TrainingPage());
            IsPresented = false;
        }

        void Training_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new ExplainPage());
            IsPresented = false;
        }

        void TOverview_Clicked(object sender, System.EventArgs e)
        {
            var user = userDBHelper.GetLoggedUser();
            Detail = new NavigationPage(new TrainingOverviewPage(user));
            IsPresented = false;
        }

        void WeeklyOverviewClicked(object sender, System.EventArgs e)
        {
            var user = userDBHelper.GetLoggedUser();
            Detail = new NavigationPage(new WeeklyOverviewPage(user));
            IsPresented = false;
            
        }

        void goals_clicked(object sender, System.EventArgs e)
        {
            var user = userDBHelper.GetLoggedUser();
            Detail = new NavigationPage(new GoalPage(user));
            IsPresented = false;

        }

        void EditUserInformationClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new EditUserInformtionPage());
            IsPresented = false;
        }
        void editUser_Clicked(object sender, System.EventArgs e)
        {
            var user = userDBHelper.GetLoggedUser();
            Detail = new NavigationPage(new EditDataPage(user));
            IsPresented = false;
        }
        

        async void LogOutClicked(object sender, System.EventArgs e)
        {
            var decision = await DisplayAlert("Achtung", "Möchten Sie sich wirklich ausloggen?", "Ja", "Nein");
            if (decision)
            {
               
                userDBHelper.LogOutUser();
                Settings.loginUser = "false";
                await Navigation.PushAsync(new LoginPage());
            }
            IsPresented = false;
        }

    }
}