using CBMTraining.Helpers;
using CBMTraining.Methods;
using CBMTraining.Models;
using CBMTraining.View.AdminPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CBMTraining.ViewModels
{
    public class UserEditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command DeleteCommand { get; set; }

        public Command SafeCommand { get; set; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        public AdminDBHelper adminDBHelper = new AdminDBHelper();

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

        public string Pwold { get; set; }

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


            if (!stringmethods.isEmpty(Pw) || !stringmethods.isEmpty(Pw2) || !stringmethods.isEmpty(Pwold))
            {
                if (string.Equals(Pw, Pw2) && string.Equals(Pwold, obj.Password))
                {
                    if(string.Equals(Pw, obj.Password))
                    {
                        await App.Current.MainPage.DisplayAlert("Achtung", "Altes Passwort darf nicht gleich dem neuem Passwort sein!", "OK");
                    }
                    else
                    {
                        obj.Password = Pw;
                        Settings.loginUser = obj.Username;
                        MessagingCenter.Send<object, string>(this, "loguser", obj.Username);
                        userDBHelper.UpdateUser(obj);
                        await App.Current.MainPage.DisplayToastAsync("Einstellung wurde gespeichert");
                    }
                }
                else if(!string.Equals(Pw, Pw2))
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
                if(userDBHelper.CheckUserexist(UserName) || adminDBHelper.CheckUserexist(UserName))
                {
                    await App.Current.MainPage.DisplayAlert("Achtung", "Username ist schon vergeben!", "OK");
                } else
                {
                    Settings.loginUser = obj.Username;
                    userDBHelper.UpdateUser(obj);
                    MessagingCenter.Send<object, string>(this, "loguser", obj.Username);
                    Console.WriteLine(obj.Username);
                    await App.Current.MainPage.DisplayToastAsync("Einstellung wurde gespeichert");

                }
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
