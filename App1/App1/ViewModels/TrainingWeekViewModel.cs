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
    public class TrainingWeekViewModel : INotifyPropertyChanged
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


        public List<int> NrOfAllImages { get; set; } = new List<int> { 0,0,0,0,0,0,0};

        public List<int> NrOfGoodImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
        public List<int> NrOfBadImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        public List<int> cmplSession { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        public List<int> quitSession { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        //Total Number
        public List<int> NrOfCorrectImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        public List<int> NrOfWrongImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };


        public List<int> NrOfGoodCorrectImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
        public List<int> NrOfGoodWrongImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
        public List<int> NrOfBadCorrectImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
        public List<int> NrOfBadWrongImages { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0 };

        // Percentage

        public double PctGoodIm { get; set; }

        public double PctBadIm { get; set; }
        public double PctCIm { get; set; }
        public double PctWIm { get; set; }
        public double PctGandCIm { get; set; }

        public double PctBandCIm { get; set; }

        public double PctGandWIm { get; set; }

        public double PctBandWIm { get; set; }

        // Time
        public List<string> ElapsedTime { get; set; } = new List<string> { "", "", "", "", "", "",""};

        public string AvgElapsedTime { get; set; }

        public string AvgT { get; set; }

        public string AvgTGPic { get; set; }

        public string AvgTBPic { get; set; }

        public string AvgTCPic { get; set; }

        public string AvgTWPic { get; set; }


        //Ticks for getting all time 
        public List<long> SessionTimeTicks { get; set; } = new List<long> { 0, 0, 0, 0, 0, 0, 0 };

        public long AvgTTicks { get; set; }

        public long AvgTGPicTicks { get; set; }
        public long AvgTBPicTicks { get; set; }
        public long AvgTCPicTicks { get; set; }

        public long AvgTWPicTicks { get; set; }


        //Total
        public double NrOfAllImagesTotal { get; set; }
        public double NrOfGoodImagesTotal { get; set; }
        public double NrOfBadImagesTotal { get; set; }
        public double NrOfCorrectImagesTotal { get; set; }
        public double NrOfWrongImagesTotal { get; set; }

        public double NrOfGoodCorrectImagesTotal { get; set; }
        public double NrOfBadCorrectImagesTotal { get; set; }
        public double NrOfGoodWrongImagesTotal { get; set; }
        public double NrOfBadWrongImagesTotal { get; set; }
        public long cmplSessionTotal { get; set; }

        public long SessionTimeTicksTotal { get; set; }

        public string ElapsedTimeTotal { get; set; }

        public int swipedir { get; set; } // swipe counter for Charts

        public TrainingWeekViewModel(User obj,DateTime start,int setChartNR)
        {
            user = obj;
            swipedir = setChartNR;
            Chart_Height = deviceMetricHelper.getHeightXamarin() / 4;
            weekstart = start;
            InitData(user, weekstart);
            TotalTSessionCommand = new Command<User>(x => OnTotal(user));
            SwipeCommand = new Command<string>(Swipe);
/*            NextCommand = new Command<string>(x => OnNext());
            LastCommand = new Command<string>(x => OnLast());*/
        }

        private async void OnTotal(User obj)
        {
            if (trainingSession.getLastCmpTrainingSessionbyUser(obj) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingTotalPage(obj));
        }

        private void Swipe(string value)
        {
            switch (value)
            {
                case "left":
                    swipedir--;
                    if (swipedir == -1)
                        swipedir = 6;
                    SetChart(swipedir);
                    break;
                case "right":
                    swipedir++;
                    if (swipedir == 7)
                    {
                        swipedir = 0;
                    }
                    SetChart(swipedir);
                    break;
            }
        }


        private void SetChart(int chartNr)
        {
            string good;
            string bad;
            if (chartNr == 2)
            {
                good = NrOfCorrectImagesTotal.ToString();
                bad = NrOfWrongImagesTotal.ToString();

            }
            else
            {
                good = NrOfCorrectImagesTotal.ToString() + " / " + String.Format("{0:0.##}%", PctCIm);
                bad = NrOfWrongImagesTotal.ToString() + " / " + String.Format("{0:0.##}%", PctWIm);
            }


            //Week Amount Training

            var weektrainamount = new Entry[7];
            for (int i = 0; i < weektrainamount.Length; i++)
            {
                weektrainamount[i] = new Entry(cmplSession[i])
                {
                    Label = String.Format("{0:dd/MM}", weekDayTime[i]),
                ValueLabel = cmplSession[i].ToString(),
                Color = SKColors.LightBlue,
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
                    Color = SKColors.LightBlue,
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
                    Color = SKColors.LightBlue,
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
                    Color = SKColors.LightBlue,
                    ValueLabelColor = SKColors.Black,
                };
            }


            //good and bad pics Chart
            var entries = new[]{
                 new Entry((float)NrOfCorrectImagesTotal)
                 {
                     Label = "Richtig gewischt",
                     ValueLabel = good,
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColor.Parse("#13fc03")
                 },
                 new Entry((float)NrOfWrongImagesTotal)
                 {
                     Label = "Falsch gewischt",
                     ValueLabel = bad,
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor = SKColor.Parse("#fc0303")
                 } };

            //correct or not and PicType Chart RadarChart
            var radarEntries = new[]{
                 new Entry((float)NrOfGoodCorrectImagesTotal)
                 {
                     Label = "r. & subst. unabh.",
                     ValueLabel = NrOfGoodCorrectImagesTotal.ToString(),
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 },
                 new Entry((float)NrOfBadCorrectImagesTotal)
                 {
                     Label = "r.& subst. abh.",
                     ValueLabel = NrOfBadCorrectImagesTotal.ToString(),
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 },

                 new Entry((float)NrOfGoodWrongImagesTotal)
                 {
                     Label = "f. & subst. unabh.",
                     ValueLabel = NrOfGoodWrongImagesTotal.ToString(),
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor = SKColors.Black
                 },

                 new Entry((float)NrOfBadWrongImagesTotal)
                 {
                     Label = "f. & subst. abh.",
                     ValueLabel = NrOfBadWrongImagesTotal.ToString(),
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor =SKColors.Black
                 }

        };


            //avg Times
            var avgtime = new[]{
                 new Entry(AvgTTicks)
                 {
                     Label = "ø Bild",
                     ValueLabel = AvgT,
                     Color = SKColors.Blue,
                     ValueLabelColor = SKColors.Black
                 },
                 new Entry(AvgTCPicTicks)
                 {
                     Label = "ø richtig",
                     ValueLabel = AvgTCPic,
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 },

                 new Entry(AvgTWPicTicks)
                 {
                     Label = "ø falsch",
                     ValueLabel = AvgTWPic,
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor = SKColors.Black
                 },

                 new Entry(AvgTGPicTicks)
                 {
                     Label = "ø subst. unabh.",
                     ValueLabel = AvgTGPic,
                     Color = SKColors.Yellow,
                     ValueLabelColor =SKColors.Black
                 },

                 new Entry(AvgTBPicTicks)
                 {
                     Label = "ø subst. abh.",
                     ValueLabel = AvgTBPic,
                     Color = SKColors.LightPink,
                     ValueLabelColor = SKColors.Black
                 },
        };

            

            float textbarsize;
            float textdonutsize;
            if (Device.RuntimePlatform == Device.UWP)
            {
                textbarsize = 20f;
                textdonutsize = 20f;
            }
            else
            {
                textbarsize = 40f;
                textdonutsize = 50f;
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
                    Charttext = "Anzahl richtig falsch gewischt";
                    Chart = new DonutChart()
                    {
                        Entries = entries,
                        HoleRadius = 0.4f,
                        LabelTextSize = textdonutsize,
                        LabelMode = LabelMode.RightOnly,
                        BackgroundColor = SKColors.AliceBlue,
                        Margin = 20,
                    };
                    break;

                case 2:
                    Charttext = "Anzahl richtig falsch gewischt";
                    Chart = new BarChart()
                    {
                        Entries = entries,
                        LabelTextSize = textbarsize,
                        MaxValue = (float)NrOfAllImagesTotal,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                case 3:
                    Charttext = "Durchschnittliche Zeiten";
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        Chart = new BarChart()
                        {
                            Entries = avgtime,
                            LabelTextSize = textbarsize,
                            ValueLabelOrientation = Orientation.Horizontal,
                            LabelOrientation = Orientation.Horizontal,
                            BackgroundColor = SKColors.AliceBlue,
                        };
                    }
                    else
                    {
                        Chart = new BarChart()
                        {
                            Entries = avgtime,
                            LabelTextSize = textbarsize,
                            ValueLabelOrientation = Orientation.Horizontal,
                            LabelOrientation = Orientation.Vertical,
                            BackgroundColor = SKColors.AliceBlue,
                        };
                    }
                    break;

                case 4:
                    Charttext = "Anzahl gewischter Bildtypen";
                    Chart = new RadarChart()
                    {
                        Entries = radarEntries,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                case 5:
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

                case 6:
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

                case 7:
                    Charttext = "Zeit pro Wochentag";
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
                if (obj.Value.IsTrainingCompleted == true)
                {


                    NrOfAllImages[Day] += obj.Value.NrOfAllImages;
                    NrOfGoodImages[Day] += obj.Value.NrOfGoodImages;
                    NrOfBadImages[Day] += obj.Value.NrOfBadImages;

                    //Total Number
                    NrOfCorrectImages[Day] += obj.Value.NrOfCorrectImages;
                    NrOfWrongImages[Day] += obj.Value.NrOfWrongImages;
                    NrOfGoodCorrectImages[Day] += obj.Value.NrOfGoodCorrectImages;
                    NrOfGoodWrongImages[Day] += obj.Value.NrOfGoodWrongImages;
                    NrOfBadCorrectImages[Day] += obj.Value.NrOfBadCorrectImages;
                    NrOfBadWrongImages[Day] += obj.Value.NrOfBadWrongImages;

                    // Time
                    SessionTimeTicks[Day] += obj.Value.SessionTimeTicks;

                    AvgTTicks += obj.Value.AvgTTicks;

                    AvgTGPicTicks += obj.Value.AvgTGPicTicks;

                    AvgTBPicTicks += obj.Value.AvgTBPicTicks;

                    AvgTCPicTicks += obj.Value.AvgTCPicTicks;

                    AvgTWPicTicks += obj.Value.AvgTWPicTicks;

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

        public void setTotalData()
        {
            

            // Percentage
            for (int i = 0; i < 7; i++)
            {
                NrOfAllImagesTotal += NrOfAllImages[i];
                NrOfGoodImagesTotal += NrOfGoodImages[i];
                NrOfBadImagesTotal += NrOfBadImages[i];
                NrOfCorrectImagesTotal += NrOfCorrectImages[i];
                NrOfWrongImagesTotal += NrOfWrongImages[i];

                NrOfGoodCorrectImagesTotal += NrOfGoodCorrectImages[i];
                NrOfBadCorrectImagesTotal += NrOfBadCorrectImages[i];
                NrOfGoodWrongImagesTotal += NrOfGoodWrongImages[i];
                NrOfBadWrongImagesTotal += NrOfBadWrongImages[i];

                cmplSessionTotal += cmplSession[i];

                SessionTimeTicksTotal += SessionTimeTicks[i];
            }
            
            //Percentage error handling
            try
            {
                PctGoodIm = (NrOfGoodImagesTotal / NrOfAllImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctGoodIm = 0;
            }
            try
            {
                PctBadIm = (NrOfBadImagesTotal / NrOfAllImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctBadIm = 0;
            }
            try
            {
                PctCIm = (NrOfCorrectImagesTotal / NrOfAllImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctCIm = 0;
            }
            try
            {
                PctWIm = (NrOfWrongImagesTotal / NrOfAllImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctGoodIm = 0;
            }
            try
            {
                PctWIm = (NrOfWrongImagesTotal / NrOfAllImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctGoodIm = 0 ;
            }
            try
            {
                PctGandCIm = (NrOfGoodCorrectImagesTotal / (double)NrOfGoodImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctGandCIm = 0;
            }
            try
            {
                PctBandCIm = (NrOfBadCorrectImagesTotal / (double)NrOfBadImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctBandCIm = 0;
            }
            try
            {
                PctGandWIm = (NrOfGoodWrongImagesTotal / (double)NrOfGoodImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctGandWIm = 0;
            }
            try
            {
                PctBandWIm = (NrOfBadWrongImagesTotal / (double)NrOfBadImagesTotal) * 100;
            }
            catch (DivideByZeroException)
            {
                PctBandWIm = 0;
            }


            try
            {
                AvgElapsedTime = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(SessionTimeTicksTotal / cmplSessionTotal));
            }
            catch (DivideByZeroException)
            {
                AvgElapsedTime = "0";
            }
            try
            {
                AvgT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTTicks / cmplSessionTotal));
            }
            catch (DivideByZeroException)
            {
                AvgT = "0";
            }
            try
            {
                AvgTGPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTGPicTicks / cmplSessionTotal));
            }
            catch (DivideByZeroException)
            {
                AvgTGPic = "0";
            }
            try
            {
                AvgTBPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTBPicTicks / cmplSessionTotal));
            }
            catch (DivideByZeroException)
            {
                AvgTBPic = "0";
            }

            try
            {
                AvgTCPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTCPicTicks / cmplSessionTotal));
            }
            catch (DivideByZeroException)
            {
                AvgTCPic = "0";
            }
            try
            {
                AvgTWPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTWPicTicks / cmplSessionTotal));
            }
            catch (DivideByZeroException)
            {
                AvgTWPic = "0";
            }

            try
            {
                ElapsedTimeTotal = stringmethods.TimeSpanToStringToHExt(TimeSpan.FromTicks(SessionTimeTicksTotal));
            }
            catch (DivideByZeroException)
            {
                ElapsedTimeTotal = "0";
            }
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
            setTotalData();
            SetDateTime(start);
            SetChart(swipedir);
        }
    }
}

