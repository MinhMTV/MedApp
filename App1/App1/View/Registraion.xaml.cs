using App1.Helpers;
using App1.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registraion : ContentPage
    {
        private int age;

        private UserDBHelper userDBHelper = new UserDBHelper();
        public User user;

        public Registraion()
        {
            InitializeComponent();
            Entry_Username.ReturnCommand = new Command(() => Entry_Firstname.Focus());
            Entry_Firstname.ReturnCommand = new Command(() => Entry_Lastname.Focus());
            Entry_Lastname.ReturnCommand = new Command(() => Entry_Email.Focus());
            Entry_Email.ReturnCommand = new Command(() => Entry_Password.Focus());
            Entry_Password.ReturnCommand = new Command(() => Entry_Repeatedpassword.Focus());
            age = (int)slider_age.Value;
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            age = Convert.ToInt32(e.NewValue);
        }

        async private void CompleteRegistration_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Entry_Username.Text) || string.IsNullOrWhiteSpace(Entry_Username.Text) ||
                string.IsNullOrEmpty(Entry_Firstname.Text) ||
                string.IsNullOrEmpty(Entry_Lastname.Text) ||
                string.IsNullOrEmpty(Entry_Email.Text) || string.IsNullOrWhiteSpace(Entry_Email.Text) ||
                string.IsNullOrEmpty(Entry_Password.Text) || string.IsNullOrWhiteSpace(Entry_Password.Text) ||
                string.IsNullOrEmpty(Entry_Repeatedpassword.Text) || string.IsNullOrWhiteSpace(Entry_Repeatedpassword.Text))
            {
                await DisplayAlert("Achtung!", "Bitte füllen Sie alle Felder aus", "OK");

            }
            else if (!string.Equals(Entry_Password.Text, Entry_Repeatedpassword.Text))
            {
                await DisplayAlert("Achtung!", "Passwörter stimmen nicht überein", "OK");
                Entry_Password.Text = string.Empty;
                Entry_Repeatedpassword.Text = string.Empty;
            }
            else
            {
                //My code
                user = new User();
                user.Username = Entry_Username.Text;
                user.Email = Entry_Email.Text;
                // To Do Please set it to zero
                user.UserID = 0;
                user.FirstName = Entry_Firstname.Text;
                user.LastName = Entry_Lastname.Text;
                user.Password = Entry_Password.Text;
                user.Age = age;
                user.IsUserLoggedIn = false;
                user.IsUserIdUpdated = false;
                user.IsUserAskedForDataProtection = false;
                user.IsDataProtectionAccepted = true;
                user.IsToDataAutoSend = true;
                user.SessionLastUpdated = DateTime.Now;
                try
                {
                    var userAddingStatus = userDBHelper.AddUser(user);
                    userDBHelper.PrintUser(user);

                    if (userAddingStatus)
                    {
                        await DisplayAlert("Glückwunsch!", "Sie haben sich erfolgreich registriert", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Achtung!", "Sie sind bereits registriert. Sie können sich einloggen.", "OK");
                    }
                    await Navigation.PushAsync(new LoginPage());
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }

            }
        }
    }
}