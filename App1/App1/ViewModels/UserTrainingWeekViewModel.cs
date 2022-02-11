using CBMTraining.Helpers;
using CBMTraining.Methods;
using CBMTraining.Models;
using CBMTraining.View.AdminPages;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;
using CBMTraining.Extensions;

namespace CBMTraining.ViewModels
{
    public class UserTrainingWeekViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();

        private Stringmethods stringmethods = new Stringmethods();
        public Chart Chart { get; set; }

        public double Chart_Height { get; set; }

        public string Charttext { get; set; }

        public Command<User> TotalTSessionCommand { get; private set; }

        //Training Properties

        public Command SwipeCommand { get; set; }

        public Command NextCommand { get; set; }

        public Command LastCommand { get; set; }

        public User user { get; private set; }

        public int UserID { get; set; }

        public List<KeyValuePair<int, TrainingSession>> Tsession { get; set; } //List of Trainingsession and Value of Week, 0 for monday - 6 for sunday

        public DateTime weekstart { get; set; }
        public List<DateTime> weekDayTime { get; set; } = new List<DateTime> { DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue };


        public List<int> NrOfAllImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        public List<int> cmplSession { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        public List<int> quitSession { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };


        // Time
        public List<string> ElapsedTime { get; set; } = new List<string> { "", "", "", "", "", "", "" };

        public string AvgElapsedTime { get; set; }


        //Ticks for getting all time 
        public List<long> SessionTimeTicks { get; set; } = new List<long> { 0, 0, 0, 0, 0, 0, 0 };



        //Total
        public double NrOfAllImagesTotal { get; set; }

        public long cmplSessionTotal { get; set; }

        public long SessionTimeTicksTotal { get; set; }

        public string ElapsedTimeTotal { get; set; }

        public int swipedir { get; set; } // swipe counter for Charts

        public UserTrainingWeekViewModel(User obj, DateTime start, int setChartNR)
        {
            user = obj;
            swipedir = setChartNR;
            Chart_Height = deviceMetricHelper.getHeightXamarin() / 2;
            weekstart = start;
            InitData(user, weekstart);
            SwipeCommand = new Command<string>(Swipe);
        }

        private void Swipe(string value)
        {
            switch (value)
            {
                case "left":
                    swipedir--;
                    if (swipedir == -1)
                        swipedir = 3;
                    SetChart(swipedir);
                    break;
                case "right":
                    swipedir++;
                    if (swipedir == 4)
                    {
                        swipedir = 0;
                    }
                    SetChart(swipedir);
                    break;
            }
        }


        private void SetChart(int chartNr)
        {

            //Week Amount Training

            var weektrainamount = new Entry[7];
            for (int i = 0; i < weektrainamount.Length; i++)
            {
                weektrainamount[i] = new Entry(cmplSession[i])
                {
                    Label = String.Format("{0:dd/MM}", weekDayTime[i]),
                    ValueLabel = cmplSession[i].ToString(),
                    Color = SKColor.Parse("#004c93"),
                    ValueLabelColor = SKColors.Black,
                };
            }

            //Week Amount Pictures

            var weekpicamount = new Entry[7];
            for (int i = 0; i < weekpicamount.Length; i++)
            {
                weekpicamount[i] = new Entry(NrOfAllImages[i])
                {
                    Label = String.Format("{0:dd/MM}", weekDayTime[i]),
                    ValueLabel = NrOfAllImages[i].ToString(),
                    Color = SKColor.Parse("#004c93"),
                    ValueLabelColor = SKColors.Black,
                };
            }

            var weektimeamount = new Entry[7];
            for (int i = 0; i < weektimeamount.Length; i++)
            {
                weektimeamount[i] = new Entry(SessionTimeTicks[i])
                {
                    Label = String.Format("{0:dd/MM}", weekDayTime[i]),
                    ValueLabel = ElapsedTime[i].ToString(),
                    Color = SKColor.Parse("#004c93"),
                    ValueLabelColor = SKColors.Black,
                };
            }

            var weekquit = new Entry[7];
            for (int i = 0; i < weekquit.Length; i++)
            {
                weekquit[i] = new Entry(quitSession[i])
                {
                    Label = String.Format("{0:dd/MM}", weekDayTime[i]),
                    ValueLabel = quitSession[i].ToString(),
                    Color = SKColor.Parse("#004c93"),
                    ValueLabelColor = SKColors.Black,
                };
            }


            float textbarsize;
            if (Device.RuntimePlatform == Device.UWP)
            {
                textbarsize = 20f;
            }
            else
            {
                textbarsize = 40f;
            }

            switch (chartNr)
            {
                case 0:
                    Charttext = "Anzahl Training pro Wochentag";
                    Chart = new BarChart()
                    {
                        Entries = weektrainamount,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                        Margin = 20,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal
                    };
                    break;

                case 1:
                    Charttext = "Anzahl abgebrochener Trainings";
                    Chart = new BarChart()
                    {
                        Entries = weekquit,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                        Margin = 20,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal
                    };
                    break;



                case 2:
                    Charttext = "Anzahl der Bilder pro Wochentag";
                    Chart = new BarChart()
                    {
                        Entries = weekpicamount,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                        Margin = 20,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal
                    };
                    break;

                case 3:
                    Charttext = "Insgesamtzeit pro Wochentag";
                    Chart = new BarChart()
                    {
                        Entries = weektimeamount,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                        Margin = 20,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelOrientation = Orientation.Horizontal
                    };
                    break;


                default:
                    break;
            }
        }

        public void setWeekData(List<KeyValuePair<int, TrainingSession>> list, int Day)
        {
            var count = 1;
            var quitcount = 1;

            var daylist = list.FindAll(list => list.Key == Day); //Liste aller Trainingsession per Tag

            foreach (var obj in daylist)
            {
                if(obj.Value.IsTrainingCompleted == true)
                {
                    NrOfAllImages[Day] += obj.Value.NrOfAllImages;

                    // Time
                    SessionTimeTicks[Day] += obj.Value.SessionTimeTicks;
                    cmplSession[Day] = count;
                    count++;
                } else
                {
                    quitSession[Day] = quitcount;
                    quitcount++;
                }
                
            }
            //Time
            ElapsedTime[Day] = stringmethods.TimeSpanToStringToH(TimeSpan.FromTicks(SessionTimeTicks[Day]));
        }



        /// <summary>
        /// get current Week which will be init
        /// </summary>
        public void SetDateTime(DateTime start)
        {
            var startDay = start;
            for (int i = 0; i < 7; i++)
            {
                weekDayTime[i] = startDay.AddDays(i);
            }
        }

        private void InitData(User user, DateTime start)
        {
            Tsession = trainingSession.GetWeekbyUserOrderANDWeekSortByDay(user, false, start);  //Default start current Monday 
            for (int x = 0; x < 7; x++)
            {
                setWeekData(Tsession, x);

            }
            SetDateTime(start);
            SetChart(swipedir);
        }
    }
}

