using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.View.AdminPages;
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

namespace CBMTraining.ViewModels
{
    public class UserDetailViewModel : BaseViewModel
    {

        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private ObservableCollection<TrainingSession> _tsession;

        public ObservableCollection<TrainingSession> TSession { get => _tsession; set => _tsession = value; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public Command TotalTSessionCommand { get; set; }

        public Command<User> WeekTSessionCommand { get; private set; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        //User Properties

        public User user { get; set; }
        public string UserName { get; set; }

        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public int SessionCount { get; set; }

        public bool isDataProtection { get; set; }

        public bool isAutoSend { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime FirstSession { get; set; }

        public UserDetailViewModel(User obj)
        {
            user = obj;
            InitData(user);
            EditCommand = new Command<User>(x=> OnEdit(user));
            DeleteCommand = new Command<User>(x => OnDelete(user));
            PressedCommand = new Command<TrainingSession>(OnPressed);
            TotalTSessionCommand = new Command<User>(x => OnTotal(user));
            WeekTSessionCommand = new Command<User>(x => OnWeek(user));
        }

        private async void OnPressed(TrainingSession obj)
        {
            await App.Current.MainPage.Navigation.PushAsync(new TrainingDetailPage(obj));
        }
        private async void OnTotal(User obj)
        {
            if (trainingSession.getLastCmpTrainingSessionbyUser(obj) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingTotalPage(obj));
        }

        private async void OnWeek(User obj)
        {
            if (trainingSession.getLastCmpTrainingSessionbyUser(user) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingWeekPage(obj));
        }


        private async void OnEdit(User obj)
        {
            await App.Current.MainPage.Navigation.PushAsync(new EditUserData(user));
        }
        private async void OnDelete(User obj)
        {
            var result = await App.Current.MainPage.DisplayAlert("Achtung!", "Wollen Sie den User wirklich löschen?", "Ja", "Nein");

            if (result)
            {
                try
                {
                    userDBHelper.DeleteAllUser();
                    await App.Current.MainPage.Navigation.PushAsync(new AdminMenu());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //do nothing
            }
        }


        private void InitData(User obj)
        {
            _tsession = new ObservableCollection<TrainingSession>();
            var templist = trainingSession.getListNrCmplSessionByUserANDOrder(obj, 3, false); //get last 3 user session order from new to old
            foreach(var temp in templist)
            {
                TSession.Add(temp);
            }
            UserName = obj.Username;
            UserID = obj.UserID;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
            Email = obj.Email;
            Age = obj.Age;
            SessionCount = trainingSession.getCompletedTrainingSessionListbyUserAndOrder(obj, false).Count;
            CreatedAt = obj.CreatedAt;
            FirstSession = obj.FirstSession;
     }
    }
}
