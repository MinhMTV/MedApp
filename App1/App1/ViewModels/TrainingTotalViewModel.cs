using App1.Helpers;
using App1.Methods;
using App1.Models;
using App1.View.AdminPages;
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

namespace App1.ViewModels
{
    public class TrainingTotalViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private Stringmethods stringmethods = new Stringmethods();

        private DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();

        public Chart Chart { get; set; }

        public double Chart_Height { get; set; }
        public Command<User> WeekTSessionCommand { get; private set; }

        public Command SwipeCommand { get; set; }

        public User user { get; private set; }

        //Training Properties

        public TrainingSession Tsession { get; set; }

        public DateTime SessionDate { get; set; }

        public int NrOfAllImages { get; set; } 

        public int NrOfGoodImages { get; set; } 
        public int NrOfBadImages { get; set; } 

        public int cmplSession { get; set; } 

        public int quitSession { get; set; }

        //Total Number
        public int NrOfCorrectImages { get; set; } 

        public int NrOfWrongImages { get; set; } 


        public int NrOfGoodCorrectImages { get; set; } 
        public int NrOfGoodWrongImages { get; set; }
        public int NrOfBadCorrectImages { get; set; } 
        public int NrOfBadWrongImages { get; set; } 

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
        public string ElapsedTime { get; set; }

        public string AvgElapsedTime { get; set; }

        public string AvgT { get; set; }

        public string AvgTGPic { get; set; }

        public string AvgTBPic { get; set; }

        public string AvgTCPic { get; set; }

        public string AvgTWPic { get; set; }


        //Ticks for getting all time 
        public long SessionTimeTicks { get; set; }

        public long AvgTTicks { get; set; }

        public long AvgTGPicTicks { get; set; }
        public long AvgTBPicTicks{ get; set; }
        public long AvgTCPicTicks{ get; set; }

        public long AvgTWPicTicks{ get; set; }

        // Feedback
        public int goodFeedback { get; set; } = 0;
        public int sadFeedback { get; set; } = 0;
        public int neutralFeedback { get; set; } = 0;

        public int swipedir { get; set; } = 0; // swipe counter for Charts


        public TrainingTotalViewModel(User obj)
        {
            user = obj;
            Chart_Height = deviceMetricHelper.getHeightXamarin() / 3;
            InitData(user);
            WeekTSessionCommand = new Command<User>(x => OnWeek(user)); 
            SwipeCommand = new Command<string>(Swipe);
        }

        private async void OnWeek(User obj)
        {
            if (trainingSession.getLastCmpTrainingSessionbyUser(obj) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingWeekPage(obj));
        }

        private void Swipe(string value)
        {
            Console.WriteLine("bevor" + swipedir);
            switch (value)
            {
                case "left":
                    swipedir--;
                    if (swipedir == -1)
                        swipedir = 4;
                    SetChart(swipedir);
                    break;
                case "right":
                    swipedir++;
                    if (swipedir == 6)
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
            if (chartNr == 1)
            {
                good = NrOfCorrectImages.ToString();
                bad = NrOfWrongImages.ToString();

            }
            else
            {
                good = NrOfCorrectImages.ToString() + " / " + String.Format("{0:0.##}%", PctCIm);
                bad = NrOfWrongImages.ToString() + " / " + String.Format("{0:0.##}%", PctWIm);
            }


            //good and bad pics Chart
            var entries = new[]{
                 new Entry(NrOfCorrectImages)
                 {
                     Label = "Richtig gewischt",
                     ValueLabel = good,
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColor.Parse("#13fc03")
                 },
                 new Entry(NrOfWrongImages)
                 {
                     Label = "Falsch gewischt",
                     ValueLabel = bad,
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor = SKColor.Parse("#fc0303")
                 } };

            //correct or not and PicType Chart RadarChart
            var radarEntries = new[]{
                 new Entry(NrOfGoodCorrectImages)
                 {
                     Label = "r. & subst. unabh.",
                     ValueLabel = NrOfGoodCorrectImages.ToString(),
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 },
                 new Entry(NrOfBadCorrectImages)
                 {
                     Label = "r.& subst. abh.",
                     ValueLabel = NrOfBadCorrectImages.ToString(),
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 },

                 new Entry(NrOfGoodWrongImages)
                 {
                     Label = "f. & subst. unabh.",
                     ValueLabel = NrOfGoodWrongImages.ToString(),
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor = SKColors.Black
                 },

                 new Entry(NrOfBadWrongImages)
                 {
                     Label = "f. & subst. abh.",
                     ValueLabel = NrOfBadWrongImages.ToString(),
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

            //quit and compl pics Chart
            var quit = new[]{
                 new Entry(quitSession)
                 {
                     Label = "Abgebrochene Trainings",
                     ValueLabel = quitSession.ToString(),
                     Color = SKColor.Parse("#fc0303"),
                     ValueLabelColor = SKColors.Black
                 },
                 new Entry(cmplSession)
                 {
                     Label = "Komplette Trainings",
                     ValueLabel = cmplSession.ToString(),
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 } };

            //Feedback Chart
            var feedbackEntries = new[]{
                 new Entry(sadFeedback)
                 {
                     Label = "schlechtes Feedback",
                     ValueLabel = sadFeedback.ToString(),
                     Color =  SKColor.Parse("#fc0303"),
                     ValueLabelColor =  SKColors.Black
                 },
                 new Entry(neutralFeedback)
                 {
                     Label = "neutrales Feedback",
                     ValueLabel = neutralFeedback.ToString(),
                     Color = SKColors.LightBlue,
                     ValueLabelColor = SKColors.Black
                 },
                 new Entry(goodFeedback)
                 {
                     Label = "gutes Feedback",
                     ValueLabel = goodFeedback.ToString(),
                     Color = SKColor.Parse("#13fc03"),
                     ValueLabelColor = SKColors.Black
                 } };


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

                case 1:
                    Chart = new BarChart()
                    {
                        Entries = quit,
                        LabelTextSize = textbarsize,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                case 2:
                    Chart = new BarChart()
                    {
                        Entries = feedbackEntries,
                        LabelTextSize = textbarsize,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                case 3:
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
                    Chart = new RadarChart()
                    {
                        Entries = radarEntries,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                case 5:
                    Chart = new BarChart()
                    {
                        Entries = entries,
                        LabelTextSize = textbarsize,
                        MaxValue = NrOfAllImages,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                default:
                    break;
            }
        }

        private void InitData(User user)
        {
            var tList = trainingSession.getAllTrainingSessionListbyUserAndOrder(user, false);

            var cmpl = 0;
            var quit = 0;
            foreach (var obj in tList)
            {
                if (obj.IsTrainingCompleted == true)
                {
                    NrOfAllImages += obj.NrOfAllImages;
                    NrOfGoodImages += obj.NrOfGoodImages;
                    NrOfBadImages += obj.NrOfBadImages;

                    //Total Number
                    NrOfCorrectImages += obj.NrOfCorrectImages;
                    NrOfWrongImages += obj.NrOfWrongImages;
                    NrOfGoodCorrectImages += obj.NrOfGoodCorrectImages;
                    NrOfGoodWrongImages += obj.NrOfGoodWrongImages;
                    NrOfBadCorrectImages += obj.NrOfBadCorrectImages;
                    NrOfBadWrongImages += obj.NrOfBadWrongImages;

                    // Time
                    SessionTimeTicks += obj.SessionTimeTicks;

                    AvgTTicks += obj.AvgTTicks;

                    AvgTGPicTicks += obj.AvgTGPicTicks;

                    AvgTBPicTicks += obj.AvgTBPicTicks;

                    AvgTCPicTicks += obj.AvgTCPicTicks;

                    AvgTWPicTicks += obj.AvgTWPicTicks;

                    //Feedback
                    if(obj.Feedback == Feedback.Bad)
                    {
                        sadFeedback++;
                        Console.WriteLine("Sad");
                    }
                    else if (obj.Feedback == Feedback.Good)
                    {
                        goodFeedback++;
                        Console.WriteLine("Good");
                    } else if(obj.Feedback == Feedback.Neutral)
                    {
                        neutralFeedback++;
                        Console.WriteLine("Neutral");
                    }
                    cmpl++;
                }
                else
                {
                    quit++;

                }
                
            }


            cmplSession = cmpl;
            quitSession = quit;


            // Percentage
            PctGoodIm = ((double)NrOfGoodImages / (double)NrOfAllImages) * 100;
            PctBadIm = ((double)NrOfBadImages / (double)NrOfAllImages) * 100;
            PctCIm = ((double)NrOfCorrectImages / (double)NrOfAllImages) * 100;
            PctWIm = ((double)NrOfWrongImages / (double)NrOfAllImages) * 100;

            PctGandCIm = ((double)NrOfGoodCorrectImages / (double)NrOfGoodImages) * 100;
            PctBandCIm = ((double)NrOfBadCorrectImages / (double)NrOfBadImages) * 100;
            PctGandWIm = ((double)NrOfGoodWrongImages / (double)NrOfGoodImages) * 100;
            PctBandWIm = ((double)NrOfBadWrongImages / (double)NrOfBadImages) * 100;


            //Time
            ElapsedTime = stringmethods.TimeSpanToStringToHExt(TimeSpan.FromTicks(SessionTimeTicks));

            AvgElapsedTime = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(SessionTimeTicks / cmplSession));

            AvgT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTTicks / cmplSession));

            AvgTGPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTGPicTicks / cmplSession));

            AvgTBPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTBPicTicks / cmplSession));

            AvgTCPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTCPicTicks / cmplSession));

            AvgTWPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(AvgTWPicTicks / cmplSession));

            SetChart(swipedir);
        }
    }
}

