using App1.Helpers;
using App1.Models;
using App1.View;
using Microcharts;
using Plugin.Connectivity;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;


namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {

        public int TotalPictures { get; set; }
        public string ElapsedTime { get; set; }
        public string AverageReactionTime { get; set; }
        public string Name { get; set; }
        public int PicturesRight { get; set; }
        public int PicturesWrong { get; set; }
        private UserDBHelper userDBHelper = new UserDBHelper();
        private TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();



        public ResultsPage()
        {
            InitializeComponent();
            var user = userDBHelper.GetLoggedUser();
            var trainingSession = trainingSessionDBHelper.getLastTrainingSessionbyUser(user);

            ElapsedTime = trainingSession.ElapsedTime;
            PicturesRight = trainingSession.NrOfCorrectImages;
            PicturesWrong = trainingSession.NrOfWrongImages;
            TotalPictures = trainingSession.NrOfAllImages;


            AverageReactionTime = trainingSession.AvgT;

            trainingSession.UserID =  userDBHelper.GetLoggedUser().UserID;

            Name = user.FirstName;
            BindingContext = this;

            List<Entry> entries = new List<Entry>
            {
                 new Entry(PicturesWrong)
                {
                    Label = "Falsche Bilder",
                    ValueLabel = PicturesWrong.ToString(),
                    Color = SKColor.Parse("#ff0000")
                },
                  new Entry(PicturesRight)
                {
                    Label = "Richtige Bilder",
                    ValueLabel = PicturesRight.ToString(),
                    Color = SKColor.Parse("#00ff00")
                }

            };
            PictureChart.Chart = new DonutChart() { Entries = entries };
            PictureChart.Chart.LabelTextSize = 35;
            PictureChart.BackgroundColor = Color.SlateGray;
        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        async private void Start_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingPage());
        }

        async private void WeeklyOverview_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeeklyOverviewPage());
        }

        async private void TrainingOverview_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingOverviewPage());
        }
    }
}
