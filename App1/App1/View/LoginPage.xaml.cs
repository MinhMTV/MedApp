using App1.Helpers;
using App1.Models;
using App1.View;
using Plugin.Connectivity;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private UserDBHelper userDBHelper;
        private AdminDBHelper adminDBHelper;
        private APIUserHelper apiUserHelper;

        public LoginPage()
        {
            InitializeComponent();
            userDBHelper = new UserDBHelper();
            apiUserHelper = new APIUserHelper();
            adminDBHelper = new AdminDBHelper();
            GlobalVariables.isAdmin = false;
        }

        // This prevents a user from being able to hit the back button and leave the login page.
        protected override bool OnBackButtonPressed()
        {
            return true;
        }


        async void LogIn(object sender, EventArgs e)
        {
            
            //Check for the username and password are not empty
            if (Entry_Username.Text != null && Entry_Password.Text != null)
            {
                
                if(!adminDBHelper.CheckUserexist(Entry_Username.Text))
                {
                    //If there is no registered user, force user to register.
                    if (!userDBHelper.CheckUserexist(Entry_Username.Text))
                    {
                        await DisplayAlert("Login", "Der Benutzer ist nicht registriert! Bitte Passwort und Benutzername überprüfen!", "Okay");
                    }
                }

                //check if admin login
                if (adminDBHelper.ValidateLogin(Entry_Username.Text, Entry_Password.Text))
                {
                    if (adminDBHelper.LogInUser(Entry_Username.Text))
                    {
                        await DisplayAlert("Login", "Admin Login erfolgreich", "Okay");
                    }
                    // Get current logged in username, if login was successfull and enable Admin View
                    //                  string userName = adminDBHelper.GetLoggedInUserName(); redundant, da der username ja schon richtig eingetippt wurde
                    Preferences.Set(constants.loginUser, Entry_Username.Text);  // set login User to Username of admin

                    //GlobalVariables.isAdmin = true;
                    //GlobalVariables.CurrentLoggedInUser = userName;
                    await Navigation.PushAsync(new AdminMenu());
                }

                //else check if user login
                // If there is any registered user, check for the validation of username and password
                else if (userDBHelper.ValidateLogin(Entry_Username.Text, Entry_Password.Text))
                {
                    if (!userDBHelper.IsUserIdUpdated())
                    {
                        //If no Internet don't let user continue
                        while (Connectivity.NetworkAccess != NetworkAccess.Internet)
                        {
                            await DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an!", "OK");
                        }

                        //Check for whether the server is up, if up try to update the user id
                        // Else inform the user, force to login
                        if (await CrossConnectivity.Current.IsRemoteReachable(GlobalVariables.ServerIP, 443))
                        {
                            UpdateUserId();
                        }
                        else
                        {
                            await DisplayAlert("Fehler!", "Der Server kann nicht erreicht werden!", "OK");
                            return;
                        }
                    }

                    if (userDBHelper.LogInUser(Entry_Username.Text))
                    {
                        await DisplayAlert("Login", "Login erfolgreich", "Okay");
                        Preferences.Set(constants.loginUser, Entry_Username.Text);  // set login User to Username of user

                    }

                    // Check whether IsDataProtection accepted
                    if (userDBHelper.IsUserAskedForDataProtection(Preferences.Get(constants.loginUser,"false")))
                    {
                        await Navigation.PushAsync(new MenuPage());
                    }
                    else await Navigation.PushAsync(new DataProtectionPage());

                }
                else
                {
                    await DisplayAlert("Login", "Benutzername oder Passwort ist falsch!", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Login", "Bitte füllen Sie alle felder aus", "Okay");
            }

        }

        private async void CheckInternetConnectionAvailability()
        {
            // Force user to get connected to the internet to complete the registration process
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an!", "OK");
            }
        }

        public async void UpdateUserId()
        {
            activity.IsEnabled = true;
            activity.IsRunning = true;
            activity.IsVisible = true;
            int returnedId;
            try
            {
                User user = userDBHelper.GetLoggedUser();
                IsBusy = true;
                returnedId = await apiUserHelper.SendUser(user);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Problem with getting response from the training database");
                Console.WriteLine(ex.StackTrace);
                return;
            }

            if (returnedId == 1)
            {
                await DisplayAlert("Server Fehler!", "Training Datenbank standard Rückgabe. Bitte den Admin melden!", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else if (returnedId == 0)
            {
                await DisplayAlert("Fehler!", "Id könnte nicht aktuallisiert werden", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                bool returnedResponse = userDBHelper.UpdateUserID(returnedId);
                if (returnedResponse)
                {
                    await DisplayAlert("OK!", " Die Patienten ID würde aktualisiert! Sie können jetzt mit dem Training anfangen", "OK");
                }
                else
                {
                    await DisplayAlert("Error!", "Contact to the admin", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            activity.IsEnabled = false;
            activity.IsRunning = false;
            activity.IsVisible = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            while ((Connectivity.NetworkAccess != NetworkAccess.Internet))
            {
                await DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an!", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }
        async void StartRegistraion(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }
    }
}