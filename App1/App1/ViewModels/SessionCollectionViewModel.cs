using App1.Helpers;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class SessionCollectionViewModel : BaseViewModel
    {

        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private ObservableCollection<TrainingSession> _tsession;

        public ObservableCollection<TrainingSession> TSession { get => _tsession; set => _tsession = value; }

        public Command RefreshCommand { get; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        User user;


        public SessionCollectionViewModel(User obj)
        {
            user = obj;
            InitData(obj);
            PressedCommand = new Command<TrainingSession>(OnPressed);
            RefreshCommand = new Command(ExecuteRefreshCommand);
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
            await App.Current.MainPage.DisplayToastAsync("Navigiere zu Trainingssession");

            //          await App.Current.MainPage.Navigation.PushAsync(new TrainingSessionDetail(obj));
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
