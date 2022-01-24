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
    public partial class TrainingOverviewPage : ContentPage
    {
        TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        public TrainingOverviewPage()
        {
            InitializeComponent();
            DateTime dateTime = DateTime.Today.Date;
            List<TrainingSession> listTS = trainingSessionDBHelper.GetLastTwoTrainingSessions();

            Console.WriteLine("Hello");

            int totalPicuresCurrent = listTS.ElementAt(0).NrOfGoodCorrectImages + listTS.ElementAt(0).NrOfGoodWrongImages +
                                        listTS.ElementAt(0).NrOfBadCorrectImages + listTS.ElementAt(0).NrOfBadWrongImages;

            int totalPicuresLast = listTS.ElementAt(1).NrOfGoodCorrectImages + listTS.ElementAt(1).NrOfGoodWrongImages +
                                    listTS.ElementAt(1).NrOfBadCorrectImages + listTS.ElementAt(1).NrOfBadWrongImages;

            int trainingTimeCurrent = Convert.ToInt32(listTS.ElementAt(0).ElapsedTime.Substring(0, 2)) * 60
                                       + Convert.ToInt32(listTS.ElementAt(0).ElapsedTime.Substring(3, 2));

            int trainingTimeLast = Convert.ToInt32(listTS.ElementAt(1).ElapsedTime.Substring(0, 2)) * 60
                                       + Convert.ToInt32(listTS.ElementAt(1).ElapsedTime.Substring(3, 2));

            int averageReactionTimeCurrent;
            int averageReactionTimeLast;

            if (trainingTimeCurrent * 10 <= 1)
            {
                averageReactionTimeCurrent = 0;
            }
            else
            {
                averageReactionTimeCurrent = trainingTimeCurrent / totalPicuresCurrent;

            }

            if (trainingTimeLast * 10 <= 1)
            {
                averageReactionTimeLast = 0;
            }
            else
            {
                averageReactionTimeLast = trainingTimeLast / totalPicuresLast;
            }



            List<Entry> entries1 = new List<Entry>
            {
                 new Entry(totalPicuresCurrent)
                {
                    Label = "Aktuell",
                    ValueLabel = totalPicuresCurrent.ToString(),
                    Color = SKColor.Parse("#00ff00")
                },
                  new Entry(totalPicuresLast)
                {
                    Label = "Letze",
                    ValueLabel = totalPicuresLast.ToString(),
                    Color = SKColor.Parse("#ff0000")
                }

            };

            List<Entry> entries2 = new List<Entry>
            {
                 new Entry(trainingTimeCurrent)
                {
                    Label = "Aktuell",
                    ValueLabel = listTS.ElementAt(0).ElapsedTime + "s",
                    Color = SKColor.Parse("#00ff00")
                },
                  new Entry(trainingTimeLast)
                {
                    Label = "Letze",
                    ValueLabel = listTS.ElementAt(1).ElapsedTime + "s",
                    Color = SKColor.Parse("#ff0000")
                }

            };

            List<Entry> entries3 = new List<Entry>
            {
                 new Entry(averageReactionTimeCurrent)
                {
                    Label = "Aktuell",
                    ValueLabel = averageReactionTimeCurrent.ToString() + "s",
                    Color = SKColor.Parse("#00ff00")
                },
                  new Entry(averageReactionTimeLast)
                {
                    Label = "Letze",
                    ValueLabel = averageReactionTimeLast.ToString() + "s",
                    Color = SKColor.Parse("#ff0000")
                }
            };

            PictureChart.Chart = new BarChart() { Entries = entries1 };
            PictureChart.Chart.LabelTextSize = 40;
            PictureChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");


            TimeChart.Chart = new BarChart() { Entries = entries2 };
            TimeChart.Chart.LabelTextSize = 40;
            TimeChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");

            ReactionTimeChart.Chart = new BarChart() { Entries = entries3 };
            ReactionTimeChart.Chart.LabelTextSize = 40;
            ReactionTimeChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");
        }
    }
}