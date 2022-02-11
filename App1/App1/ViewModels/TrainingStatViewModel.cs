using CBMTraining.Helpers;
using CBMTraining.Methods;
using CBMTraining.Models;
using CBMTraining.View.AdminPages;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;

namespace CBMTraining.ViewModels
{
    public class TrainingStatViewModel : INotifyPropertyChanged
    {
        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private DeviceMetricHelper deviceMetricHelper = new DeviceMetricHelper();

        public event PropertyChangedEventHandler PropertyChanged;
        public Chart Chart { get; set; }

        public double Chart_Height { get; set; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public Command SwipeCommand { get; set; }


        public Command NextCommand { get; set; }

        public Command LastCommand { get; set; }
        public Command<User> TotalTSessionCommand { get; private set; }

        public Command<User> WeekTSessionCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        public User user { get; private set; }

        //Training Properties

        public TrainingSession Tsession { get; set; }

        public DateTime SessionDate { get; set; }

        public int NrOfAllImages { get; set; }

        public int NrOfGoodImages { get; set; }
        public int NrOfBadImages { get; set; }

        public int cmplSession { get; set; }

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
        public long SessionTimeTicks { get; set; } //Time for finishing TrainingSession as Ticks
        public string ElapsedTime { get; set; } //Time for finishing TrainingSession (conversion from Ticks to string for better reading) min,sec,ms

        public long AvgTTicks { get; set; }//average time for Pic as Ticks

        public string AvgT { get; set; }//average time for Pic in min,sec,ms



        public long AvgTGPicTicks { get; set; }//average time for good pics as Ticks

        public string AvgTGPic { get; set; } //average time for good pics in min,sec,ms



        public long AvgTBPicTicks { get; set; }//average time for Bad pics as Ticks

        public string AvgTBPic { get; set; } //average time for bad pics in min,sec,ms



        public long AvgTCPicTicks { get; set; }//average time for correct Pics in Ticks
        public string AvgTCPic { get; set; } //average time for correct pics in min,sec,ms



        public long AvgTWPicTicks { get; set; }//average time for wrong pics as Ticks
        public string AvgTWPic { get; set; } //average time for wrong pics in min,sec,ms





        public TrainingStatViewModel(TrainingSession obj)
        {
            Tsession = obj;
            Chart_Height = deviceMetricHelper.getHeightXamarin() / 3.5;
            InitData(Tsession);
            TotalTSessionCommand = new Command<User>(x => OnTotal(user));
            WeekTSessionCommand = new Command<User>(x => OnWeek(user));
            SwipeCommand = new Command<string>(Swipe);
            NextCommand = new Command<string>(x => OnNext());
            LastCommand = new Command<string>(x => OnLast());

        }

        private async void OnLast()
        {
            var ts = trainingSession.getBeforeCmplTrainingbyUserANDSession(user, Tsession);
            if (ts != null)
            {
                Tsession = ts;
                InitData(Tsession);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Achtung", "Erstes Training schon erreicht", "OK");
            }
            
        }



        private async void OnNext()
        {
            var ts = trainingSession.getNextCmplTrainingbyUserANDSession(user, Tsession);
            if (ts != null)
            {
                Tsession = ts;
                InitData(Tsession);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Achtung", "Letztes Training schon erreicht", "OK");
            }
        }

        public int swipedir { get; set; } = 0;

        private void Swipe(string value)
        {
            Console.WriteLine("bevor" + swipedir);
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
                    if(swipedir == 4 )
                    {
                        swipedir = 0;
                    }
                    SetChart(swipedir);
                    break;
            }
        }

        private async void OnTotal(User obj)
        {
            if (trainingSession.getLastCmpTrainingSessionbyUser(obj) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingTotalPage(obj));
        }

        private async void OnWeek(User obj)
        {
            if (trainingSession.getLastCmpTrainingSessionbyUser(obj) == null)
                await App.Current.MainPage.DisplayAlert("Achtung", "User hat bisher noch kein Training absolviert für eine Statistik", "Ok");
            else
                await App.Current.MainPage.Navigation.PushAsync(new TrainingWeekPage(obj));
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
                    Chart = new DonutChart() {
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
                        Entries = entries,
                        LabelTextSize = textbarsize,
                        MaxValue = NrOfAllImages, 
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelOrientation = Orientation.Horizontal,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                case 2:
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
                    } else
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

                case 3:
                    Chart = new RadarChart()
                    {
                        Entries = radarEntries,
                        LabelTextSize = textbarsize,
                        BackgroundColor = SKColors.AliceBlue,
                    };
                    break;

                default:
                    break;
            }


        }


        private void InitData(TrainingSession obj)
        {
            user = userDBHelper.GetUserByUserID(obj.UserID);

            SessionDate = obj.SessionDate;

            NrOfAllImages = obj.NrOfAllImages;

            NrOfGoodImages = obj.NrOfGoodImages;
            NrOfBadImages = obj.NrOfBadImages;

            cmplSession = obj.cmplSession;

        //Total Number
            NrOfCorrectImages = obj.NrOfCorrectImages;

            NrOfWrongImages  = obj.NrOfWrongImages;


            NrOfGoodCorrectImages = obj.NrOfGoodCorrectImages;
            NrOfGoodWrongImages = obj.NrOfGoodWrongImages;
            NrOfBadCorrectImages = obj.NrOfBadCorrectImages;
            NrOfBadWrongImages = obj.NrOfBadWrongImages;

            // Percentage
            PctGoodIm = obj.PctGoodIm;

            PctBadIm = obj.PctBadIm;
            PctCIm = obj.PctCIm;
            PctWIm = obj.PctWIm;


            PctGandCIm = obj.PctGandCIm;

            PctBandCIm = obj.PctBandCIm;

            PctGandWIm = obj.PctGandWIm;

            PctBandWIm = obj.PctBandWIm;

            // Time
            SessionTimeTicks = obj.SessionTimeTicks;
            ElapsedTime = obj.ElapsedTime;

            AvgTTicks = obj.AvgTTicks;
            AvgT = obj.AvgT;

            AvgTGPicTicks = obj.AvgTGPicTicks;
            AvgTGPic = obj.AvgTGPic;

            AvgTBPicTicks = obj.AvgTBPicTicks;
            AvgTBPic = obj.AvgTBPic;

            AvgTCPicTicks = obj.AvgTCPicTicks;
            AvgTCPic = obj.AvgTCPic;

            AvgTWPicTicks = obj.AvgTWPicTicks;
            AvgTWPic = obj.AvgTWPic;

            SetChart(swipedir);

        }
    }
}

