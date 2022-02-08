using App1.Models;
using App1.ViewModels;
using MLToolkit.Forms.SwipeCardView;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Helpers;
using App1.Extensions;
using App1.Methods;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace App1.View.UserPages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage : ContentPage
    {
        
        private bool isUp;
        private bool isRunning;
        private int totalImages;
        public TrainingSession trainingSession;
        private User user;
        static int interval = 200; // 200ms interval check for timer
        static long TotalTime = 0; // Elapsed time in ms

        PicType currentPicType;
        private ObservableCollection<Pictures> newCollection;
        private PictureDBHelper pictureDBHelper = new PictureDBHelper();
        private TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        private UserDBHelper userDBHelper = new UserDBHelper();
        private PicTimeDBHelper picTimeDBHelper = new PicTimeDBHelper();

        private Stringmethods stringmethods = new Stringmethods();
        private Stopwatch stopwatch2 = new Stopwatch();

        private static Timer timer;

        int indexer = 0;
        public TrainingPage()
        {
            this.InitializeComponent();
            this.BindingContext = new SwipeViewModel();
            SwipeCardView.Dragging += OnDragging;
            trainingSession = new TrainingSession();

            var lastTraining = trainingSessionDBHelper.GetLastTrainingSession();
            if (lastTraining != null)
            {
                trainingSession.SessionId = lastTraining.SessionId + 1;
                Console.WriteLine(lastTraining.SessionId);
            }     
            else
                trainingSession.SessionId = 1;
            Console.WriteLine(trainingSession.SessionId);
            user = userDBHelper.GetLoggedUser();
            trainingSession.SessionDate = DateTime.Now.AddDays(-4);
            trainingSession.IsTrainingCompleted = false;
            totalImages = pictureDBHelper.GetAllImagesToList().Count;

            newCollection = (this.BindingContext as SwipeViewModel).Pictures;
            GlobalVariables.isNavigation = true;
            isRunning = false;

            if (GlobalVariables.isTimer)
            {
                TotalTime = (GlobalVariables.defaultSec * 1000) + (GlobalVariables.defaultMin * 60000); //Total time by User in ms
                Console.WriteLine(TotalTime);
                timer = new Timer(interval);
                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                timer.Enabled = true;
            }
            

            startStopWatches();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            
            timer.Stop();
            if (GlobalVariables.isNavigation == false)
            {
                timer.Enabled = false;
                Console.WriteLine("Timer wurde gestoppt");
            } 
            else
            {
                stopwatch2.Stop();
                GlobalVariables.Stopwatch.Stop();
                Console.WriteLine(GlobalVariables.Stopwatch.Elapsed);
            
                if (GlobalVariables.Stopwatch.ElapsedMilliseconds > TotalTime)
                {
                    trainingSession.IsTrainingCompleted = true;
                    getStatistic();
                    resetStopWatches();
                    Navigation.PushAsync(new ResultsPage());
                }
                else
                {
               
                    timer.Enabled = true; //Or timer.Start();
                
                }
                if(isRunning == true)
                {
                    startStopWatches();
                }
            }

        }

        private void startStopWatches()
        {
            GlobalVariables.Stopwatch.Start();
            stopwatch2.Start();
            isRunning = true;
        }

        private void stopStopWatches()
        {
            GlobalVariables.Stopwatch.Stop();
            stopwatch2.Stop();
            isRunning = false;
        }

        private void resetStopWatches()
        {
            GlobalVariables.Stopwatch.Reset();
            stopwatch2.Reset();
            isRunning = false;
        }


        public void getStatistic()
        {
            trainingSession.UserID = user.UserID;
            trainingSession.ElapsedTime = stringmethods.TimeSpanToStringToMinExt(GlobalVariables.Stopwatch.Elapsed);
            trainingSession.SessionTimeTicks = GlobalVariables.Stopwatch.ElapsedTicks;
            var piclist = picTimeDBHelper.getAllPictimebyTrainingSession(trainingSession);
            Console.WriteLine("Count" + piclist.Count);
            long goodTicks = 0;
            int goodPics = 0;
            int badPics = 0;
            long badTicks = 0;
            long correctPicsTicks = 0;
            long wrongPicsTicks = 0;
            long correctAndGoodPicTicks = 0;
            long correctAndBadPicTicks = 0;
            long wrongAndGoodPicTicks = 0;
            long wrongAndBadPicTicks = 0;

            foreach (var pic in piclist)
            {
                if(pic.Type == PicType.Good)
                {
                    goodTicks += pic.TimeTicks;
                    goodPics++;
                    if(pic.CorrectImage == true)
                    {
                        correctPicsTicks += pic.TimeTicks;
                        correctAndGoodPicTicks += pic.TimeTicks;
                    } else if(pic.CorrectImage == false)
                    {
                        wrongPicsTicks += pic.TimeTicks;
                        wrongAndGoodPicTicks += pic.TimeTicks;
                    }

                } else if(pic.Type ==PicType.Bad)
                {
                    badTicks += pic.TimeTicks;
                    badPics++;
                    if (pic.CorrectImage == true)
                    {
                        correctPicsTicks += pic.TimeTicks;
                        correctAndBadPicTicks += pic.TimeTicks;
                    }
                    else if (pic.CorrectImage == false)
                    {
                        wrongPicsTicks += pic.TimeTicks;
                        wrongAndBadPicTicks += pic.TimeTicks;
                    }
                }
            }
            trainingSession.gImageT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(goodTicks));
            trainingSession.bImageT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(badTicks));
            trainingSession.CImageT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(correctPicsTicks));

            trainingSession.WImageT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongPicsTicks));

            trainingSession.GAndCT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(correctAndGoodPicTicks));

            trainingSession.BAndCT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(correctAndBadPicTicks));

            trainingSession.GAndWT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongAndGoodPicTicks));

            trainingSession.BAndWT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongAndBadPicTicks));

            // See Explanation of all Statistic in TrainingSession Model
            try
            {
                trainingSession.AvgTTicks = GlobalVariables.Stopwatch.ElapsedTicks / trainingSession.NrOfAllImages;
                trainingSession.AvgT = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(GlobalVariables.Stopwatch.ElapsedTicks / trainingSession.NrOfAllImages));
            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTTicks = 0;
                trainingSession.AvgT = null;
            }
            

            try
            {
                trainingSession.AvgTGPicTicks = goodTicks / goodPics;
                trainingSession.AvgTGPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(goodTicks / goodPics));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTGPicTicks = 0;
                trainingSession.AvgTGPic = null;
            }
            

            try
            {
                trainingSession.AvgTBPicTicks = badTicks / badPics;
                trainingSession.AvgTBPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(badTicks / badPics));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTGPicTicks = 0;
                trainingSession.AvgTBPic = null;
            }
            

            try
            {
                trainingSession.AvgTCPicTicks = correctPicsTicks / (trainingSession.NrOfGoodCorrectImages + trainingSession.NrOfBadCorrectImages);
                trainingSession.AvgTCPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(correctPicsTicks / (trainingSession.NrOfGoodCorrectImages + trainingSession.NrOfBadCorrectImages)));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTCPicTicks = 0;
                trainingSession.AvgTCPic = null;
            }
            

            try
            {
                trainingSession.AvgTWPicTicks = wrongPicsTicks / (trainingSession.NrOfGoodWrongImages + trainingSession.NrOfBadWrongImages);
                trainingSession.AvgTWPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongPicsTicks / (trainingSession.NrOfGoodWrongImages + trainingSession.NrOfBadWrongImages)));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTWPicTicks = 0;
                trainingSession.AvgTWPic = null;
            }
            


            try
            {
                trainingSession.AvgTGAndCPicTicks = correctAndGoodPicTicks / trainingSession.NrOfGoodCorrectImages;
                trainingSession.AvgTGAndCPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(correctAndGoodPicTicks / trainingSession.NrOfGoodCorrectImages));


            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTGAndCPicTicks = 0;
                trainingSession.AvgTGAndCPic = null;
            }
            


            try
            {
                trainingSession.AvgTBAndCPicTicks = correctAndBadPicTicks / trainingSession.NrOfBadCorrectImages;
                trainingSession.AvgTBAndCPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(correctAndBadPicTicks / trainingSession.NrOfBadCorrectImages));
            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTBAndCPicTicks = 0;
                trainingSession.AvgTBAndCPic = null;
            }
            


            try
            {
                trainingSession.AvgTGAndWPicTicks = wrongAndGoodPicTicks / trainingSession.NrOfGoodWrongImages;
                trainingSession.AvgTGAndWPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongAndGoodPicTicks / trainingSession.NrOfGoodWrongImages));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTGAndWPicTicks = 0;
                trainingSession.AvgTGAndWPic = null;
            }
            


            try
            {
                trainingSession.AvgTGAndWPicTicks = wrongAndGoodPicTicks / trainingSession.NrOfGoodWrongImages;
                trainingSession.AvgTGAndWPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongAndGoodPicTicks / trainingSession.NrOfGoodWrongImages));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTGAndWPicTicks = 0;
                trainingSession.AvgTGAndWPic = null;
            }
            

            try
            {
                trainingSession.AvgTBAndWPicTicks = wrongAndBadPicTicks / trainingSession.NrOfBadWrongImages;
                trainingSession.AvgTBAndWPic = stringmethods.TimeSpanToStringToMinExt(TimeSpan.FromTicks(wrongAndBadPicTicks / trainingSession.NrOfBadWrongImages));

            }
            catch (DivideByZeroException)
            {
                trainingSession.AvgTBAndWPicTicks = 0;
                trainingSession.AvgTBAndWPic = null;
            }

            if (trainingSession.IsTrainingCompleted == true)
            {
                try
                {
                    trainingSession.cmplSession = trainingSessionDBHelper.getCompletedTrainingSessionListbyUserAndOrder(user, false).Count + 1;
                }
                catch 
                {
                    trainingSession.cmplSession = 1;
                }
                
            }
            trainingSessionDBHelper.AddTrainingSession(trainingSession);
        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void OnDragging(object sender, DraggingCardEventArgs e)
        {
            if (indexer == totalImages)
            {
                indexer += 1;
            }

            switch (e.Position)
            {
                case DraggingCardPosition.Start:

                    break;
                case DraggingCardPosition.UnderThreshold:
                    if (e.Direction == SwipeCardDirection.Up)
                    {
                    }
                    else if (e.Direction == SwipeCardDirection.Down)
                    {
                    }
                    break;
                case DraggingCardPosition.OverThreshold:


                    if (e.Direction == SwipeCardDirection.Up)
                    {
                        isUp = true;
                    }
                    // If swipped down and picture type is good
                    else if (e.Direction == SwipeCardDirection.Down)
                    {
                        isUp = false;
                    }
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    break;

                case DraggingCardPosition.FinishedOverThreshold:

                    currentPicType = (this.BindingContext as SwipeViewModel).Pictures[indexer].Type;
                    indexer += 1;
                    Console.WriteLine("currentPictype" + currentPicType.ToString());
                    Console.WriteLine("indexer" + indexer.ToString());

                    if (isUp)
                    {
                        // Picture was moved up and the type is also bad
                        if (currentPicType == PicType.Bad)
                        {
                            stopwatch2.Stop();
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMinExt(stopwatch2.Elapsed);
                            picTime.Type = PicType.Bad;
                            picTime.CorrectImage = true;
                            picTimeDBHelper.AddPicTime(picTime);
                            trainingSession.NrOfBadCorrectImages += 1;
                            stopwatch2.Restart();
                        }
                        // Picture was moved up but the type was good
                        else
                        {
                            stopwatch2.Stop();
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMinExt(stopwatch2.Elapsed);
                            picTime.Type = PicType.Good;
                            picTime.CorrectImage = false;
                            picTimeDBHelper.AddPicTime(picTime);
                            trainingSession.NrOfGoodWrongImages += 1;
                            stopwatch2.Restart();
                        }

                    }
                    else
                    {
                        // Picture was moved down and the type is also good
                        if (currentPicType == PicType.Good)
                        {
                            stopwatch2.Stop();
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMinExt(stopwatch2.Elapsed);
                            picTime.Type = PicType.Good;
                            picTime.CorrectImage = true;
                            picTimeDBHelper.AddPicTime(picTime);
                            trainingSession.NrOfGoodCorrectImages += 1;
                            stopwatch2.Restart();
                        }
                        // Picture was moved down but the type was bad
                        else
                        {
                            stopwatch2.Stop();
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMinExt(stopwatch2.Elapsed);
                            picTime.Type = PicType.Bad;
                            picTime.CorrectImage = false;
                            picTimeDBHelper.AddPicTime(picTime);
                            trainingSession.NrOfBadWrongImages += 1;
                            stopwatch2.Restart();
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (indexer == totalImages && GlobalVariables.isNavigation == true || indexer == GlobalVariables.defaultPicCount && GlobalVariables.isNavigation == true)
            {
                indexer = 0;
                stopStopWatches();
                trainingSession.IsTrainingCompleted = true;
                GlobalVariables.isNavigation = false;
                getStatistic();
                resetStopWatches();
                await Task.Delay(500);
                await Navigation.PushAsync(new ResultsPage());
            }
        }


        public async void Exit_button_Clicked(object sender, EventArgs e)
        {
            stopStopWatches();
            var result = await DisplayAlert("Exit", "Do you want to end the test?", "Yes", "No");

            if (result == true) //if yes is true\
            {
                indexer = 0;
                trainingSession.IsTrainingCompleted = false;
                GlobalVariables.isNavigation = false;
                getStatistic();
                GlobalVariables.Stopwatch.Reset();
                stopwatch2.Reset();
                await Navigation.PushAsync(new MenuPage());

            }
            else
            {
                //Methode um Timer weiterzufuehren
                startStopWatches();
            }
            return;
        }
    }
}