using App1.Helpers;
using App1.Models;
using App1.Methods;
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
        private Stringmethods stringmethods = new Stringmethods();
        public Admin admin;

        public AdminReg()
        {
            InitializeComponent();
            Entry_Entity.ReturnCommand = new Command(() => Entry_Entity.Focus());
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
            if (stringmethods.isEmpty(Entry_Entity.Text))
            {
                missEntity = true;
            }
            //username is missing
            if (stringmethods.isEmpty(Entry_Username.Text))
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
            else if (adminDBHelper.CheckUserexist(Entry_Username.Text))
            {
                await DisplayAlert("Achtung!", "Benutzer ist schon registriert!", "OK");
            }
            else
            {
                //My code
                admin = new Admin();
                admin.Entity = Entry_Entity.Text;
                admin.Username = Entry_Username.Text.ToLower();
                admin.Email = Entry_Email.Text;
                admin.FirstName = Entry_Firstname.Text;
                admin.LastName = Entry_Lastname.Text;
                admin.Password = Entry_Password.Text;
                try
                {
                    var userAddingStatus = adminDBHelper.AddUser(admin,Entry_Username.Text);
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