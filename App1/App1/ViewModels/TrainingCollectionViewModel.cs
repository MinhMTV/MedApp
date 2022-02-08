using App1.Helpers;
using App1.Models;
using App1.View.AdminPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class TrainingCollectionViewModel : BaseViewModel
    {

        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private ObservableCollection<TrainingSession> _tsession;

        public ObservableCollection<TrainingSession> TSession { get => _tsession; set => _tsession = value; }

        public Command RefreshCommand { get; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public Command<User> TotalTSessionCommand { get; private set; }

        public Command<User> WeekTSessionCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        User user;


        public TrainingCollectionViewModel(User obj)
        {
            user = obj;
            InitData(obj);
            PressedCommand = new Command<TrainingSession>(OnPressed);
            RefreshCommand = new Command(ExecuteRefreshCommand);
            TotalTSessionCommand = new Command<User>(x => OnTotal(user));
            WeekTSessionCommand = new Command<User>(x => OnWeek(user));
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                SetProperty(ref isRefreshing, value);
            }
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
            if (trainingSession.getLastCmpTrainingSessionbyUser(obj) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingWeekPage(obj));
        }

        private void ExecuteRefreshCommand()
        {
            IsRefreshing = true;

            foreach (var item in trainingSession.getCompletedTrainingSessionListbyUserAndOrder(user, false))
            {
                _tsession.Add(item);
            }
            // Stop refreshing
            IsRefreshing = false;
        }

        private async void OnPressed(TrainingSession obj)
        {
            await App.Current.MainPage.Navigation.PushAsync(new TrainingDetailPage(obj));
        }


        private void InitData(User obj)
        {
            _tsession = new ObservableCollection<TrainingSession>();
            var templist = trainingSession.getCompletedTrainingSessionListbyUserAndOrder(obj,false); 
            foreach (var temp in templist)
            {
                TSession.Add(temp);
            }


        }
    }
}
