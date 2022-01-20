using App1.Helpers;
using App1.Methods;
using App1.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        private int age;

        private UserDBHelper userDBHelper = new UserDBHelper();
        private AdminDBHelper adminDBHelper = new AdminDBHelper();
        private Stringmethods stringmethods = new Stringmethods();
        public User user;
        

        public Registration()
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
            var missUsern = false;
            var missEntity = false;
            var missName = false;
            var missNameorEnt = false;
            var missMail = false;
            var missPw = false;
            var username = Entry_Username.Text.ToLower().Trim();

            if (stringmethods.isEmpty(username))
            {
                missUsern = true;
            }
            
            {
                //cant use isempty method cause names have whitespace sometimes
                if (string.IsNullOrEmpty(Entry_Firstname.Text) || string.IsNullOrEmpty(Entry_Lastname.Text)) 
                missName = true;
            }

            if (missName && missEntity == true)
            {
                missNameorEnt = true;
            }

            if (stringmethods.isEmpty(Entry_Email.Text))
            {
                missMail = true;
            }

            if (stringmethods.isEmpty(Entry_Password.Text) || stringmethods.isEmpty(Entry_Repeatedpassword.Text))
            {
                missPw = true;
            }

            if (missMail || missPw || missNameorEnt == true)
            {
                await DisplayAlert("Achtung!", "Bitte Füllen Sie alle erforderlichen Felder aus", "OK");
                if (missUsern) { Entry_Username.PlaceholderColor = Color.Red; Entry_Username.Placeholder = "Bitte Username angeben!"; }
                if (missNameorEnt) { Entry_Lastname.PlaceholderColor = Color.Red; Entry_Lastname.Placeholder = "Bitte Name angeben!";  }
                if (missMail) { Entry_Email.PlaceholderColor = Color.Red; Entry_Email.Placeholder = "Bitte E-Mail angeben!"; }
                if (missPw) { Entry_Password.PlaceholderColor = Color.Red; Entry_Password.Placeholder = "Bitte Passwort angeben!"; }

            }
            else if (!string.Equals(Entry_Password.Text, Entry_Repeatedpassword.Text))
            {
                await DisplayAlert("Achtung!", "Passwörter stimmen nicht überein", "OK");
                Entry_Password.Text = string.Empty;
                Entry_Repeatedpassword.Text = string.Empty;
            }
            else if (adminDBHelper.CheckUserexist(Entry_Username.Text) || userDBHelper.CheckUserexist(Entry_Username.Text))
            {
                await DisplayAlert("Achtung!", "Benutzer ist schon registriert!", "OK");
            }
            else if (username == Entry_Password.Text.ToLower().Trim() || username == Entry_Firstname.Text.ToLower().Trim() ||
                username == Entry_Lastname.Text.ToLower().Trim())
            {
                await DisplayAlert("Achtung!", "Name und Passwort sollte nicht gleich sein!", "OK");
            }
            else 
            {
                //My code
                user = new User();
                user.Username = username;
                user.Email = Entry_Email.Text;
                // Generate random userID for the patient
                var userid = 0;

                do
                {
                    userid = userDBHelper.GenerateUserID();
                    user.UserID = userid;
                }
                while (!userDBHelper.IsUserIDUnique(userid));

                user.FirstName = Entry_Firstname.Text;
                user.LastName = Entry_Lastname.Text;
                user.Password = Entry_Password.Text;
                user.Age = age;
                user.IsUserIdUpdated = false;
                user.IsUserAskedForDataProtection = false;
                user.IsDataProtectionAccepted = true;
                user.IsToDataAutoSend = true;
                user.SessionLastUpdated = DateTime.Now;
                userDBHelper.PrintUser(user);

                try
                {
                    var userAddingStatus = userDBHelper.AddUser(user, username);
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
