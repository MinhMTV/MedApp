using App1.Helpers;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserInformtionPage : ContentPage
    {
        UserDBHelper userDBHelper = new UserDBHelper();
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
                if (userDBHelper.GetUserPassword().Equals(Entry_OldPassword.Text))
                {
                    if (userDBHelper.SetUserPassword(Entry_NewPassword.Text))
                    {
                        await DisplayAlert("Erfolg", "Passwort wurde geändert!", "Okay");
                        Entry_OldPassword.Text = string.Empty;
                        Entry_NewPassword.Text = string.Empty;
                        Entry_ConfirmPassword.Text = string.Empty;
                    }
                }
                //worng old password
                else
                {
                    await DisplayAlert("Fehler", "Bitte tragen Sie das richtige alte Passwort", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Fehler", "Ihr neue Passwörter stimmen nicht überein", "Okay");
            }
        }
    }
}