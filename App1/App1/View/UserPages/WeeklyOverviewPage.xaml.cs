using App1.Helpers;
using App1.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeeklyOverviewPage : ContentPage
    {
        TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        List<Entry> entries1 = new List<Entry>();
        List<Entry> entries2 = new List<Entry>();
        List<Entry> entries3 = new List<Entry>();
        List<Entry> entries4 = new List<Entry>();
        public WeeklyOverviewPage()
        {
            InitializeComponent();
            DateTime dateTime = DateTime.Today.Date;
            List<TrainingSession> listTS = trainingSessionDBHelper.GetLastTrainingSessions();

            for (int i = 0; i < 7; i++)
            {
                var getElement = listTS.FindAll(x => x.SessionDate == dateTime);
                int timeTracker = 0;
                int nrOfPictures = 0;
                bool isTrainingCompleted = false;
                string parsedDate;
                foreach (TrainingSession trainingSession in getElement)
                {
                    timeTracker = timeTracker + Convert.ToInt32(trainingSession.ElapsedTime.Substring(0, 2)) * 60
                                       + Convert.ToInt32(trainingSession.ElapsedTime.Substring(3, 2));
                    nrOfPictures = nrOfPictures + trainingSession.NrOfGoodCorrectImages + trainingSession.NrOfGoodWrongImages
                                                + trainingSession.NrOfBadCorrectImages + trainingSession.NrOfBadWrongImages;
                    isTrainingCompleted = trainingSession.IsTrainingCompleted; //Last training of the day will be taken
                }

                //Prepare date
                parsedDate = getElement.First().SessionDate.Day.ToString() +
                                    "/" + getElement.First().SessionDate.Month.ToString();


                //Does user has trained?
                entries1.Add(new Entry((nrOfPictures != 0) ? 1 : 0)
                {

                    Label = parsedDate,
                    ValueLabel = (nrOfPictures != 0) ? "Ja" : "Nein",
                    Color = SKColor.Parse("#0000ff")
                });

                //How long?
                entries2.Add(new Entry(timeTracker)
                {

                    Label = parsedDate,
                    ValueLabel = timeTracker.ToString() + "s",
                    Color = SKColor.Parse("#0000ff")
                });

                //How many picures?
                entries3.Add(new Entry(nrOfPictures)
                {

                    Label = parsedDate,
                    ValueLabel = nrOfPictures.ToString(),
                    Color = SKColor.Parse("#0000ff")
                });

                //How many time a training was not completed?
                entries4.Add(new Entry((isTrainingCompleted == true) ? 1 : 0)
                {

                    Label = parsedDate,
                    ValueLabel = (isTrainingCompleted == true) ? "Ja" : "Nein",
                    Color = SKColor.Parse("#0000ff")
                });


                dateTime = dateTime.AddDays(-1);
            }


            PrsenceChart.Chart = new BarChart() { Entries = entries1 };
            PrsenceChart.Chart.LabelTextSize = 40;
            PrsenceChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");

            TimeChart.Chart = new LineChart() { Entries = entries2 };
            TimeChart.Chart.LabelTextSize = 40;
            TimeChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");

            PictureChart.Chart = new LineChart() { Entries = entries3 };
            PictureChart.Chart.LabelTextSize = 40;
            PictureChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");

            CancellationChart.Chart = new BarChart() { Entries = entries4 };
            CancellationChart.Chart.LabelTextSize = 40;
            CancellationChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");
        }
    }
}
