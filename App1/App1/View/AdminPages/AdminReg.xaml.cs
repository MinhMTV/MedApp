using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.Methods;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CBMTraining.View.GeneralPages;

namespace CBMTraining.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminReg : ContentPage
    {
        private AdminDBHelper adminDBHelper = new AdminDBHelper();
        private Stringmethods stringmethods = new Stringmethods();
        public Admin admin;
        private String username = "";

        public AdminReg()
        {
            InitializeComponent();
        }

        async private void CompleteRegistration_Clicked(object sender, EventArgs e)
        {
            var missUsern = false;
            var missEntity = false;
            var missName = false;
            var missNameorEnt = false;
            var missMail = false;
            var missPw = false;

            if (!string.IsNullOrEmpty(Entry_Firstname.Text))
            {
                username = Entry_Username.Text.ToLower().Trim();
            }

            //username is missing
            if (stringmethods.isEmpty(Entry_Entity.Text))
            {
                missEntity = true;
            }
            //username is missing
            if (stringmethods.isEmpty(username))
            {
                missUsern = true;
            }
             //cant use isempty method cause names have whitespace sometimes
            if (string.IsNullOrEmpty(Entry_Firstname.Text) || string.IsNullOrEmpty(Entry_Lastname.Text))
            { 
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

            // name and entity is missing
            if (missName && missEntity == true)
            {
                missNameorEnt = true;
            }

            //check if username/mail/passwort or either name or entity is missing
            if (missUsern || missMail || missPw || missNameorEnt == true)
            {
                await DisplayAlert("Achtung!", "Bitte Füllen Sie alle erforderlichen Felder aus", "OK");
                if (missUsern) { Entry_Username.PlaceholderColor = Color.Red; Entry_Username.Placeholder = "Bitte Username angeben!"; }
                if (missNameorEnt) { Entry_Lastname.PlaceholderColor = Color.Red; Entry_Lastname.Placeholder = "Bitte Name angeben!"; Entry_Entity.PlaceholderColor = Color.Red; Entry_Entity.Placeholder = "Bitte Name angeben!"; }
                if (missMail) { Entry_Email.PlaceholderColor = Color.Red; Entry_Email.Placeholder = "Bitte E-Mail angeben!"; }
                if (missPw) { Entry_Password.PlaceholderColor = Color.Red; Entry_Password.Placeholder = "Bitte Passwort angeben!"; }
                
            }
            else if (!string.Equals(Entry_Password.Text, Entry_Repeatedpassword.Text))
            {
                await DisplayAlert("Achtung!", "Passwörter stimmen nicht überein", "OK");
                Entry_Password.Text = string.Empty;
                Entry_Repeatedpassword.Text = string.Empty;
            }
            else if (adminDBHelper.CheckUserexist(username))
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
                admin = new Admin();
                admin.Entity = Entry_Entity.Text;
                admin.Username = username;
                admin.Email = Entry_Email.Text;
                admin.FirstName = Entry_Firstname.Text;
                admin.LastName = Entry_Lastname.Text;
                admin.Password = Entry_Password.Text;
                admin.CreatedAt = DateTime.Now;
                try
                {
                    var userAddingStatus = adminDBHelper.AddUser(admin, username);
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