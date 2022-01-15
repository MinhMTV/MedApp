using App1.Helpers;
using App1.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminReg : ContentPage
    {
        private AdminDBHelper adminDBHelper = new AdminDBHelper();
        public Admin admin;

        public AdminReg()
        {
            InitializeComponent();
            Entry_Unternehmen.ReturnCommand = new Command(() => Entry_Unternehmen.Focus());
            Entry_Username.ReturnCommand = new Command(() => Entry_Firstname.Focus());
            Entry_Firstname.ReturnCommand = new Command(() => Entry_Lastname.Focus());
            Entry_Lastname.ReturnCommand = new Command(() => Entry_Email.Focus());
            Entry_Email.ReturnCommand = new Command(() => Entry_Password.Focus());
            Entry_Password.ReturnCommand = new Command(() => Entry_Repeatedpassword.Focus());
        }

        async private void CompleteRegistration_Clicked(object sender, EventArgs e)
        {
            var missUsern = false;
            var missEntity = false;
            var missName = false;
            var missNameorEnt = false;
            var missMail = false;
            var missPw = false;

            //username is missing
            if (string.IsNullOrEmpty(Entry_Username.Text) || string.IsNullOrWhiteSpace(Entry_Username.Text))
            {
                missUsern = true;
            }
            //entity is missing
            if (string.IsNullOrEmpty(Entry_Unternehmen.Text) || string.IsNullOrWhiteSpace(Entry_Unternehmen.Text)) {
                 missEntity = true;
            }

            //name is missing
            if (string.IsNullOrEmpty(Entry_Firstname.Text) && string.IsNullOrEmpty(Entry_Lastname.Text))
            {
                 missName = true;
            }

            // name and entity is missing
            if (missName && missEntity == true)
            {
                missNameorEnt = true;
            }
            //mail is missing
            if (string.IsNullOrEmpty(Entry_Email.Text) || string.IsNullOrWhiteSpace(Entry_Email.Text))
            {
                missMail = true;
            }

            //password is missing
            if (string.IsNullOrEmpty(Entry_Password.Text) || string.IsNullOrWhiteSpace(Entry_Password.Text) ||
                string.IsNullOrEmpty(Entry_Repeatedpassword.Text) || string.IsNullOrWhiteSpace(Entry_Repeatedpassword.Text))
            {
                missPw = true;
            }

            //check if username/mail/passwort or either name or entity is missing
            if (missUsern || missMail || missPw || missNameorEnt == true)
            {
                await DisplayAlert("Achtung!", "Bitte Füllen Sie alle erforderlichen Felder aus", "OK");
                if (missUsern) { Entry_Username.PlaceholderColor = Color.Red; Entry_Username.Placeholder = "Bitte Username angeben!"; }
                if (missNameorEnt) { Entry_Lastname.PlaceholderColor = Color.Red; Entry_Lastname.Placeholder = "Bitte Name angeben!"; Entry_Unternehmen.PlaceholderColor = Color.Red; Entry_Unternehmen.Placeholder = "Bitte Name angeben!"; }
                if (missMail) { Entry_Email.PlaceholderColor = Color.Red; Entry_Email.Placeholder = "Bitte E-Mail angeben!"; }
                if (missPw) { Entry_Password.PlaceholderColor = Color.Red; Entry_Password.Placeholder = "Bitte Passwort angeben!"; }
                
            }
            else if (!string.Equals(Entry_Password.Text, Entry_Repeatedpassword.Text))
            {
                await DisplayAlert("Achtung!", "Passwörter stimmen nicht überein", "OK");
                Entry_Password.Text = string.Empty;
                Entry_Repeatedpassword.Text = string.Empty;
            }
            else if (adminDBHelper.CheckUserexist(Entry_Username.Text))
            {
                await DisplayAlert("Achtung!", "Benutzer ist schon registriert!", "OK");
            }
            else
            {
                //My code
                admin = new Admin();
                admin.Username = Entry_Username.Text;
                admin.Email = Entry_Email.Text;
                admin.FirstName = Entry_Firstname.Text;
                admin.LastName = Entry_Lastname.Text;
                admin.Password = Entry_Password.Text;
                admin.IsUserLoggedIn = false;
                try
                {
                    var userAddingStatus = adminDBHelper.AddAdmin(admin,Entry_Username.Text);
                    adminDBHelper.PrintUser(admin);

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