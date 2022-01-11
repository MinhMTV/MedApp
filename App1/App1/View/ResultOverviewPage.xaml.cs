using App1.Helpers;
using App1.Models;
using Microcharts;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace App1.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultOverviewPage : MasterDetailPage
    {
        TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        TrainingSession trainingSession = new TrainingSession();


        public ResultOverviewPage()
        {
            InitializeComponent();

            // Prepare data to display 
            //Debug.WriteLine(trainingSessionDBHelper.DeleteAllTSession());

            //int j = 1;
            //for (int i = 23; i <30; i++)
            //{
            //    TrainingSession trainingSession = new TrainingSession();
            //    trainingSession.SessionDate = i.ToString() + ".12.2019";
            //    trainingSession.UserId = 0;
            //    trainingSession.ElapsedTime = j.ToString();
            //    j += 1;
            //    trainingSessionDBHelper.AddTrainingSession(trainingSession);
            //}
            List<TrainingSession> trainingSessions = new List<TrainingSession>();
            trainingSessions = trainingSessionDBHelper.GetTrainingSessions();

            List<Entry> listToShow = new List<Entry>();

            //foreach(TrainingSession tSession in trainingSessions)
            //{
            //    listToShow.Add( new Entry(Convert.ToInt32(tSession.ElapsedTime))
            //    {
            //        Label = tSession.SessionDate,
            //        ValueLabel = tSession.ElapsedTime +  "m",
            //        Color = SKColor.Parse("#266489")
            //    });
            //}

            //List<Entry> entries = new List<Entry>
            //{
            //     new Entry(1)
            //    {
            //        Label = "26.12.2019",
            //        ValueLabel ="60s",
            //        Color = SKColor.Parse("#266489")
            //    },
            //      new Entry(0)
            //    {
            //        Label = "27.12.2019",
            //        ValueLabel ="0s",
            //        Color = SKColor.Parse("#68B9C0")
            //    },
            //        new Entry(3)
            //    {
            //        Label = "28.12.2019",
            //        ValueLabel ="180s",
            //        Color = SKColor.Parse("#68B9C0")
            //    },
            //      new Entry(2)
            //    {
            //        Label = "29.12.2019",
            //        ValueLabel ="120s",
            //        Color = SKColor.Parse("#90D585")
            //    },
            //        new Entry(1)
            //    {
            //        Label = "30.12.2019",
            //        ValueLabel ="60s",
            //        Color = SKColor.Parse("#91D585")
            //    }

            //};

            WeeklyChart.Chart = new BarChart() { Entries = listToShow };
        }
    }
}