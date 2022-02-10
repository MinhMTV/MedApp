using App1.Helpers;
using App1.Methods;
using App1.Models;
using Microcharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using Entry = Microcharts.ChartEntry;

namespace App1.ViewModels
{
    class UserTrainingResultViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();

        public Chart ChartPic { get; set; }

        public Chart ChartTime { get; set; }

        public double Chart_Height { get; set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        public User user { get; private set; }

        //Training Properties

        public List<TrainingSession> Tsession { get; set; }

        public List<String> ElapsedTime { get; set; } = new List<String> { "", "" };

        public List<long> SessionTimeTicks { get; set; } = new List<long> { 0, 0 };
        public List<int> NrOfAllImages { get; set; } = new List<int> { 0,0};


        public UserTrainingResultViewModel(User obj)
        {
            user = obj;
            Chart_Height = deviceMetricHelper.getHeightXamarin() / 4;
            InitData(user);
        }

        private void InitData(User obj)
        {
            var tlist = trainingSession.getLastTwoCmplTrainingSessionbyUser(user);

            if (tlist == null)
            {
                Console.WriteLine("No last Session");
            }
            else
            {   
                for (int i = 0; i < tlist.Count; i++)
                {
                    if(tlist[i] != null)
                    {
                        ElapsedTime[i] = tlist[i].ElapsedTime;
                        NrOfAllImages[i] = tlist[i].NrOfAllImages;
                        SessionTimeTicks[i] = tlist[i].SessionTimeTicks;
                    }
                }
            }

            SetChart(); 
        }

        private void SetChart()
        {
            

            float textbarsize;
            if (Device.RuntimePlatform == Device.UWP)
            {
                textbarsize = 20f;
            }
            else
            {
                textbarsize = 40f;
            }

            var TimeEntries = new List<Entry>
            {
                 new Entry((float) SessionTimeTicks[1])
                {
                    Label = "Letztes Training",
                    ValueLabel = ElapsedTime[1],
                    Color = SKColors.Red
                },
                  new Entry((float) SessionTimeTicks[0])
                {
                    Label = "Aktuelles Training",
                    ValueLabel = ElapsedTime[0],
                    Color = SKColors.Green
                }

            };

            var PicEntries = new List<Entry>
            {
                 new Entry((float) NrOfAllImages[1])
                {
                    Label = "Letztes Training",
                    ValueLabel = NrOfAllImages[1].ToString(),
                    Color = SKColors.Red
                },
                  new Entry((float) NrOfAllImages[0])
                {
                    Label = "Aktuelles Training",
                    ValueLabel = NrOfAllImages[0].ToString(),
                    Color = SKColors.Green
                }

            };

                    ChartPic = new BarChart()
                    {
                        Entries = PicEntries,
                        LabelTextSize = textbarsize,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        Margin = 20,
                    };

                    ChartTime = new BarChart()
                    {
                        Entries = TimeEntries,
                        LabelTextSize = textbarsize,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.AliceBlue,
                    };
        }

    }
}
