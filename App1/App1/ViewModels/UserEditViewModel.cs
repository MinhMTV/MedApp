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
    public class UserEditViewModel : BaseViewModel
    {
        public Command DeleteCommand { get; set; }

        public Command SafeCommand { get; set; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        public User User { get; set; }

        //User Properties
        public string UserName { get; set; }

        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string Pw { get;set; }

        public string Pw2 { get; set; }

        Stringmethods stringmethods = new Stringmethods();


        public UserEditViewModel(User obj)
        {
            InitData(obj);
            DeleteCommand = new Command<User>(OnDelete);
            SafeCommand = new Command<User>(x => OnSafe(obj));
        }

        private async void OnDelete(User obj)
        {
            var result = await App.Current.MainPage.DisplayAlert("Achtung", "Möchten Sie den User wirklich löschen?", "Ja", "Nein");
            if (result)
            {
                userDBHelper.DeleteAllUser();
                await App.Current.MainPage.Navigation.PushAsync(new UserCollectionPage());
            }
        }

        private async void OnSafe(User obj)
        {
            obj.Username = UserName;
            obj.UserID = UserID;
            obj.FirstName = FirstName;
            obj.LastName = LastName;
            obj.Email = Email;
            obj.Age = Age;

            if (!stringmethods.isEmpty(Pw) || !stringmethods.isEmpty(Pw2))
            {
                if(string.Equals(Pw, Pw2))
                {
                    obj.Password = Pw;
                    userDBHelper.UpdateUser(obj);
                    await App.Current.MainPage.DisplayToastAsync("Einstellung wurde gespeichert");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Achtung", "Passwörter stimmen nicht überein!", "OK");
                }

            }
            else
            {
                userDBHelper.UpdateUser(obj);
                await App.Current.MainPage.DisplayToastAsync("Einstellung wurde gespeichert");
            }
        }


        private void InitData(User obj)
        {
            User = obj;
            UserName = obj.Username;
            UserID = obj.UserID;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
            Email = obj.Email;
            Age = obj.Age;
        }
    }
}
