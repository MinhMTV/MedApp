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
            name.Text = "Hello Admin " + loguser;
            SendTrainingSessions();
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
        }

        void PhotoGalleryClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new PhotoGalleryOverView());
            IsPresented = false;
        }

        void EditUserInformationClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new EditUserInformtionPage());
            IsPresented = false;
        }
        void EditDataProtectionClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new EditDataProtectionPage());
            IsPresented = false;
        }

        void LocalNotificationClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new LocalNotificationPage());
            IsPresented = false;
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
        }

        async void SendTrainingSessions()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an, um die Daten automatisch zu senden", "OK");
            }
            else
            {
                //Check for whether the server is up, if up try to update the user id
                // Else inform the user, force to login
                if (await CrossConnectivity.Current.IsRemoteReachable(GlobalVariables.ServerIP, 443))
                {
                    TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
                    APITSHelper apiTSHelper = new APITSHelper();
                    List<TrainingSession> trainingSessions = trainingSessionDBHelper.GetUnsentTrainingSessions();

                    if (trainingSessions.Count != 0)
                    {
                        foreach (TrainingSession ts in trainingSessions)
                        {
                            if (await apiTSHelper.SendTrainingSession(ts))
                            {
                                trainingSessionDBHelper.setDataSent(ts.SessionId);
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Fehler!", "Der Server kann nicht erreicht werden!", "OK");
                }

            }
        }

    }
}