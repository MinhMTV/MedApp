using App1.Helpers;
using App1.Models;
using App1.View;
using App1.View.AdminPages;
using App1.View.UserPages;
using Plugin.Connectivity;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.GeneralPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private UserDBHelper userDBHelper;
        private AdminDBHelper adminDBHelper;

        public LoginPage()
        {
            InitializeComponent();
            userDBHelper = new UserDBHelper();
            adminDBHelper = new AdminDBHelper();
        }

        // This prevents a user from being able to hit the back button and leave the login page.
        protected override bool OnBackButtonPressed()
        {
            return true;
        }


        async void LogIn(object sender, EventArgs e)
        {
            
            //Check for the username and password are not empty
            if (Entry_Username.Text != null || Entry_Password.Text != null)
            {
                var username = Entry_Username.Text.ToLower().TrimEnd().TrimStart();

                if (!adminDBHelper.CheckUserexist(username))
                {
                    //If there is no registered user, force user to register.
                    if (!userDBHelper.CheckUserexist(username))
                    {
                        await DisplayAlert("Login", "Der Benutzer ist nicht registriert! Bitte Passwort und Benutzername überprüfen!", "Okay");
                        return;
                    }
                }

                if (adminDBHelper.ValidateLogin(username,Entry_Password.Text))
                {
                    await DisplayAlert("Login", "Admin Login erfolgreich", "Okay");
                    Preferences.Set(constants.loginUser, username);  // set login User to Username of admin
                    await Navigation.PushAsync(new AdminMenu());
                }

                //else check if user login
                // If there is any registered user, check for the validation of username and password and login if true
                else if (userDBHelper.ValidateLogin(username, Entry_Password.Text))
                {
                    await DisplayAlert("Login", "Login erfolgreich", "Okay");
                    Preferences.Set(constants.loginUser, username);  // set login User to Username of user
                    var user = userDBHelper.GetLoggedUser();

                    if (user.isAskDataProtec)
                    {
                        await Navigation.PushAsync(new MenuPage());
                    } 
                    else
                    {
                        await Navigation.PushAsync(new DataProtectionPage());
                    }

                }
                else
                {
                    await DisplayAlert("Login", "Benutzername oder Passwort ist falsch!", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Login", "Bitte Benutzername oder Passwort eingeben", "Okay");
            }

        }


        async void StartRegistration(object sender, EventArgs e)
        {
            if (userDBHelper.checkNoUser())
                await Navigation.PushModalAsync(new Registration(false));
            else
                await DisplayAlert("Achtung", "Ein Nutzer ist schon registriert. Bitte Therapeut um Löschung des Users anfragen", "OK");
        }
    }
}