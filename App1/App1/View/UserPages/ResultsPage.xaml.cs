﻿using App1.Helpers;
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
        public double AverageReactionTime { get; set; }
        public string Name { get; set; }
        public int PicturesRight { get; set; }
        public int PicturesWrong { get; set; }
        private UserDBHelper userDBHelper = new UserDBHelper();
        private TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        private APITSHelper apiTSHelper = new APITSHelper();
        private bool isDone;



        public ResultsPage(TrainingSession trainingSession, bool isTrainingCompleted)
        {
            InitializeComponent();
            GlobalVariables.TimeSpend = GlobalVariables.Stopwatch.Elapsed;

            //ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
            //    GlobalVariables.TimeSpend.Minutes, GlobalVariables.TimeSpend.Seconds,
            //GlobalVariables.TimeSpend.Milliseconds);
            ElapsedTime = String.Format("{0:00}:{1:00}",
                GlobalVariables.TimeSpend.Minutes, GlobalVariables.TimeSpend.Seconds);
            PicturesRight = trainingSession.NrOfGoodCorrectImages + trainingSession.NrOfBadCorrectImages;
            PicturesWrong = trainingSession.NrOfGoodWrongImages + trainingSession.NrOfBadWrongImages;
            TotalPictures = PicturesRight + PicturesWrong;


            AverageReactionTime = Math.Round(((Convert.ToDouble(ElapsedTime.Substring(0, 2)) * 60 +
                                               Convert.ToDouble(ElapsedTime.Substring(3, 2))) /
                                                TotalPictures),
                                                2);

            trainingSession.PatientId = Convert.ToInt32(userDBHelper.getLoggedinUserProperty("userid"));

            // Image infromations are already added
            trainingSession.ElapsedTime = ElapsedTime;
            trainingSession.AverageReactionTime = AverageReactionTime;
            trainingSession.IsTrainingCompleted = isTrainingCompleted;
            trainingSession.IsDataSent = false;
            trainingSession.SessionDate = DateTime.Now.Date;

            trainingSessionDBHelper.AddTrainingSession(trainingSession);

            GlobalVariables.Stopwatch.Reset();
            GlobalVariables.Stopwatch.Stop();


            //Task.Run(async () =>
            //{
            //    isDone = await apiTSHelper.SendTrainingSession(trainingSession);
            //});
            //apiTSHelper.SendTrainingSession(trainingSession);

            //if (await apiTSHelper.SendTrainingSession(trainingSession))
            //{
            //    DisplayAlert("Erfolg", "Die Trainingsdaten wurden geschikt", "Ok");
            //}

            //Name = Application.Current.Properties["Name"].ToString();
            Name = userDBHelper.getLoggedinUserProperty("firstname");
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

            SendTainingSession(trainingSession);
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

        async private void SendTainingSession(TrainingSession trainingSession)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Keine Internetverbindung", "Aktivieren Sie WLAN oder mobile Daten, um die Daten automatisch zu senden!", "OK");
            }
            else
            {
                //Check for whether the server is up, if up try to update the user id
                // Else inform the user, force to login
                if (await CrossConnectivity.Current.IsRemoteReachable(GlobalVariables.ServerIP, 443))
                {
                    if (await apiTSHelper.SendTrainingSession(trainingSession))
                    {

                        await DisplayAlert("Erfolg!", "Die Trainingsdaten wurden versendet!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Fehler!", "Der Server kann nicht erreicht werden!", "OK");
                }

            }
        }

        private void CheckInternetConnection(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Keine Internetverbindung", "Aktivieren Sie  WLAN oder mobile Daten, um die Daten automatisch zu senden!", "OK");
            }

        }


    }
}