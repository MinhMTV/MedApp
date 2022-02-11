using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.View;
using Microcharts;
using Plugin.Connectivity;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;


namespace CBMTraining.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {

        public int TotalPictures { get; set; }
        public string ElapsedTime { get; set; }
        public string AverageReactionTime { get; set; }
        public string Name { get; set; }

        public bool isPokal { get; set; }

        public bool isNextPokal { get; set; }

        public int countUntilPokal { get; set; }

        public bool IsBackVisible { get; set; } = true;

        private UserDBHelper userDBHelper = new UserDBHelper();
        private TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        private TrainingSession tsession;
        private User user;



        public ResultsPage()
        {
            InitializeComponent();
            user = userDBHelper.GetLoggedUser();
            tsession  = trainingSessionDBHelper.getLastTrainingSessionbyUser(user);

            ElapsedTime = tsession.ElapsedTime;
            TotalPictures = tsession.NrOfAllImages;

            var tCount = trainingSessionDBHelper.getCompletedTrainingSessionListbyUserAndOrder(user, false).Count;
            Console.WriteLine(tCount.ToString());

            if (tCount % Settings.defaultCupCount == 0)
            {
                isPokal = true;
                isNextPokal = false;
            } else
            {
                countUntilPokal = Settings.defaultCupCount - (tCount % Settings.defaultCupCount) ;
                isPokal = false;
                isNextPokal = true;
            }
            Name = user.FirstName;
            BindingContext = this;

            if(Device.RuntimePlatform == Device.UWP)
            {
                IsBackVisible = false;
            }

        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        async private void OnSad_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.DisplayToastAsync("Danke für dein Feedback!");
            tsession.Feedback = Feedback.Bad;
            trainingSessionDBHelper.UpdateTraining(tsession);
        }

        async private void OnGood_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.DisplayToastAsync("Danke für dein Feedback!");
            tsession.Feedback = Feedback.Good;
            trainingSessionDBHelper.UpdateTraining(tsession);
        }


        async private void OnNeutral_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.DisplayToastAsync("Danke für dein Feedback!");
            tsession.Feedback = Feedback.Neutral;
            trainingSessionDBHelper.UpdateTraining(tsession);
        }


        async private void Start_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingPage());
        }

        async private void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage());
        }

        async private void WeeklyOverview_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeeklyOverviewPage(user));
        }

        async private void TrainingOverview_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingOverviewPage(user));
        }
    }
}
