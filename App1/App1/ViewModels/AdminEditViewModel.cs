using App1.Helpers;
using App1.Methods;
using App1.Models;
using App1.View.AdminPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace App1.ViewModels
{
    public class AdminEditViewModel : BaseViewModel
    {
        public Command SafeCommand { get; set; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public AdminDBHelper adminDBHelper = new AdminDBHelper();

        public UserDBHelper userDBHelper = new UserDBHelper();

        public Admin admin { get; set; }

        //Admin Properties

        
        public string UserName { get; set; }

        public string Entity { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Pw { get; set; }

        public string Pw2 { get; set; }

        public string Pwold { get; set; }

        Stringmethods stringmethods = new Stringmethods();


        public AdminEditViewModel(Admin obj)
        {
            InitData(obj);
            SafeCommand = new Command<Admin>(x => OnSafe(obj));
        }

        private async void OnSafe(Admin obj)
        {
            obj.Username = UserName;
            obj.FirstName = FirstName;
            obj.LastName = LastName;
            obj.Email = Email;
            obj.Entity = Entity;


            if (!stringmethods.isEmpty(Pw) || !stringmethods.isEmpty(Pw2) || !stringmethods.isEmpty(Pwold))
            {
                if (string.Equals(Pw, Pw2) && string.Equals(Pwold, obj.Password))
                {
                    if (string.Equals(Pw, obj.Password))
                    {
                        await App.Current.MainPage.DisplayAlert("Achtung", "Altes Passwort darf nicht gleich dem neuem Passwort sein!", "OK");
                    }
                    else
                    {
                        obj.Password = Pw;
                        Settings.loginUser = obj.Username;
                        adminDBHelper.UpdateUser(obj);
                        await App.Current.MainPage.DisplayToastAsync("Einstellung wurde gespeichert");
                    }
                }
                else if (!string.Equals(Pw, Pw2))
                {
                    await App.Current.MainPage.DisplayAlert("Achtung", "Passwörter stimmen nicht überein!", "OK");
                }
                else if (!string.Equals(Pwold, obj.Password))
                {
                    await App.Current.MainPage.DisplayAlert("Achtung", "Altes Passwort ist nicht korrekt!", "OK");
                }

            }
            else
            {
                    if (userDBHelper.CheckUserexist(UserName) || adminDBHelper.CheckUserexist(UserName))
                    {
                        await App.Current.MainPage.DisplayAlert("Achtung", "Username ist schon vergeben!", "OK");
                    }
                    else
                    {
                        Settings.loginUser = obj.Username;
                        adminDBHelper.UpdateUser(obj);
                        await App.Current.MainPage.DisplayToastAsync("Einstellung wurde gespeichert");

                    }
                }
        }


        private void InitData(Admin obj)
        {
            admin = obj;
            UserName = obj.Username;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
            Email = obj.Email;
            Entity = obj.Entity;
        }
    }
}
