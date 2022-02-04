using App1.Helpers;
using App1.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserInformtionPage : ContentPage
    {
        private UserDBHelper userDBHelper = new UserDBHelper();
        private AdminDBHelper adminDBHelper = new AdminDBHelper();
        public EditUserInformtionPage()
        {
            InitializeComponent();
            
        }
        async void OK_Clicked(object sender, EventArgs e)
        {
            //If new password entries match
            if (Entry_NewPassword.Text.Equals(Entry_ConfirmPassword.Text))
            {
               
                //If right old password entered
                if (userDBHelper.GetLoggedUser().Password.Equals(Entry_OldPassword.Text))
                {
                    if(Entry_NewPassword.Text.Equals(Entry_OldPassword.Text))
                    {
                        await DisplayAlert("Achtung", "Bitte gib ein neues Passwort ein", "Okay");
                    }
                    else if (userDBHelper.SetUserPassword(Preferences.Get(constants.loginUser,"false"),Entry_NewPassword.Text))
                    {
                        await DisplayAlert("Erfolg", "Passwort wurde geändert!", "Okay");
                        Entry_OldPassword.Text = string.Empty;
                        Entry_NewPassword.Text = string.Empty;
                        Entry_ConfirmPassword.Text = string.Empty;
                    }
                    else
                    {
                        throw new Exception("something went wrong...");
                    }
                }
                
                else if (adminDBHelper.getLoggedinUserProperty("password").Equals(Entry_OldPassword.Text))
                {
                    if (Entry_NewPassword.Text.Equals(Entry_OldPassword.Text))
                    {
                        await DisplayAlert("Achtung", "Bitte gib ein neues Passwort ein", "Okay");
                    }
                    else if (adminDBHelper.SetUserPassword(Preferences.Get(constants.loginUser, "false"), Entry_NewPassword.Text))
                    {
                        await DisplayAlert("Erfolg", "Passwort wurde geändert!", "Okay");
                        Entry_OldPassword.Text = string.Empty;
                        Entry_NewPassword.Text = string.Empty;
                        Entry_ConfirmPassword.Text = string.Empty;
                    }
                    else
                    {
                        throw new Exception("something went wrong...");
                    }
                }
                else //worng old password
                {
                    await DisplayAlert("Fehler", "Bitte tragen Sie das richtige alte Passwort ein", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Fehler", "Ihr neuen Passwörter stimmen nicht überein", "Okay");
            }
        }
    }
}