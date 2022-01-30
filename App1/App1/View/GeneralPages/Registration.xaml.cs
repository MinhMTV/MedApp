using App1.Helpers;
using App1.Methods;
using App1.Models;
using App1.View;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.GeneralPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        private int age;

        private UserDBHelper userDBHelper = new UserDBHelper();
        private AdminDBHelper adminDBHelper = new AdminDBHelper();
        private Stringmethods stringmethods = new Stringmethods();
        public User user;
        private String username = "";
        private bool isAdmin = false;

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

        public Registration(bool passingData)
        {
            InitializeComponent();
            Entry_Username.ReturnCommand = new Command(() => Entry_Firstname.Focus());
            Entry_Firstname.ReturnCommand = new Command(() => Entry_Lastname.Focus());
            Entry_Lastname.ReturnCommand = new Command(() => Entry_Email.Focus());
            Entry_Email.ReturnCommand = new Command(() => Entry_Password.Focus());
            Entry_Password.ReturnCommand = new Command(() => Entry_Repeatedpassword.Focus());
            age = (int)slider_age.Value;
            if(passingData)
            {
                isAdmin = true;
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            age = Convert.ToInt32(e.NewValue);
        }

        async private void CompleteRegistration_Clicked(object sender, EventArgs e)
        {
            var missUsern = false;
            var missFirstName = false;
            var missLastname = false;
            var missMail = false;
            var missPw = false;

            if (!string.IsNullOrEmpty(Entry_Username.Text))
            {
                username = Entry_Username.Text.ToLower().Trim();
            }
            

            if (stringmethods.isEmpty(username))
            {
                missUsern = true;
            }
            
            {
                //cant use isempty method cause names have whitespace sometimes
                if (string.IsNullOrEmpty(Entry_Firstname.Text))
                    missFirstName = true;
            }

            if (stringmethods.isEmpty(Entry_Lastname.Text))
            {
                missLastname = true;
            }

            if (stringmethods.isEmpty(Entry_Email.Text))
            {
                missMail = true;
            }

            if (stringmethods.isEmpty(Entry_Password.Text) || stringmethods.isEmpty(Entry_Repeatedpassword.Text))
            {
                missPw = true;
            }

            if (missUsern || missMail || missPw || missFirstName || missLastname == true)
            {
                await DisplayAlert("Achtung!", "Bitte Füllen Sie alle erforderlichen Felder aus", "OK");
                if (missUsern) { Entry_Username.PlaceholderColor = Color.Red; Entry_Username.Placeholder = "Bitte Username angeben!"; }
                if (missLastname) { Entry_Lastname.PlaceholderColor = Color.Red; Entry_Lastname.Placeholder = "Bitte Name angeben!";  }
                if (missFirstName) { Entry_Firstname.PlaceholderColor = Color.Red; Entry_Firstname.Placeholder = "Bitte Name angeben!"; }
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
            else if (username == Entry_Password.Text.ToLower().Trim() || Entry_Firstname.Text.ToLower().Trim() == Entry_Password.Text.ToLower().Trim() ||
                Entry_Lastname.Text.ToLower().Trim() == Entry_Password.Text.ToLower().Trim())
            {
                await DisplayAlert("Achtung!", "Name oder Passwort sollten nicht gleich sein!", "OK");
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
                user.FirstSession = DateTime.MinValue;
                user.LastSession = DateTime.MaxValue;
                user.CreatedAt = DateTime.Now;
                user.Start = DateTime.Now;
                user.End = DateTime.MaxValue; //therapyEnd will be maximum value
                user.SessionTimeMin = GlobalVariables.defaultMin; //Session will last 5 min as default
                user.SessionTimeSec = GlobalVariables.defaultSec; //Session will last 0 additional seconds as default
                //default time can be changed by admin

                userDBHelper.PrintUser(user);

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
                    if(isAdmin)
                    {
                        await Navigation.PopModalAsync();
                    } else
                    {
                        await Navigation.PushAsync(new LoginPage());
                    }
                    
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }

            }
        }
    }
}
