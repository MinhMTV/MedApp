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

namespace App1.View.UserPages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage : ContentPage
    {
        private const int defaultTimespan = 1;
        public string elapsedTime;
        private bool isUp;
        public TrainingSession trainingSession;
        
        PicType currentPicType;
        ObservableCollection<Pictures> newCollection;
        private PictureDBHelper pictureDBHelper = new PictureDBHelper();
        private TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();
        private UserDBHelper userDBHelper = new UserDBHelper();
        private PicTimeDBHelper picTimeDBHelper = new PicTimeDBHelper();

        private Stringmethods stringmethods;
        private Stopwatch stopwatch2;

        int indexer = 0;
        public TrainingPage()
        {
            this.InitializeComponent();
            this.BindingContext = new SwipeViewModel();
            SwipeCardView.Dragging += OnDragging;
            trainingSession = new TrainingSession();
            trainingSession.SessionId = trainingSessionDBHelper.GetAllTrainingsSessionToList().Count + 1;
            trainingSession.SessionDate = DateTime.Now;

            newCollection = (this.BindingContext as SwipeViewModel).Pictures;
            GlobalVariables.isNavigation = true;
            stopwatch2 = new Stopwatch();


            if (!GlobalVariables.Stopwatch.IsRunning)
            {
                GlobalVariables.Stopwatch.Start();
                stopwatch2.Start();
            }

            Device.StartTimer(new TimeSpan(0, defaultTimespan, 0), () =>
            {
                // Logic for logging out if the device is inactive for a period of time.

                if (GlobalVariables.Stopwatch.IsRunning && GlobalVariables.Stopwatch.Elapsed.Minutes >= defaultTimespan)
                {
                    //prepare to perform your data pull here as we have hit the 5 minute mark  Gerade ist 1 Minute eingestellt: s. defaultTimespan 
                    GlobalVariables.Stopwatch.Stop();
                    stopwatch2.Stop();
                    GlobalVariables.isNavigation = false;
                    Navigation.PushAsync(new ResultsPage(trainingSession, true));
                }
                // Always return true as to keep our device timer running.
                return true;
            });
        }

        public void getStatistic()
        {
            trainingSession.UserID = userDBHelper.GetLoggedUser().UserID;
            trainingSession.ElapsedTime = stringmethods.TimeSpanToStringToMin(GlobalVariables.Stopwatch.Elapsed);
            trainingSession.SessionTimeTicks = GlobalVariables.Stopwatch.ElapsedTicks;
            trainingSession.AvgTTicks = GlobalVariables.Stopwatch.ElapsedTicks / trainingSession.NrOfAllImages;
            trainingSession.AvgT = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(GlobalVariables.Stopwatch.ElapsedTicks / trainingSession.NrOfAllImages));
            var piclist = picTimeDBHelper.getAllPictimebyTrainingSession(trainingSession);
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

            trainingSession.AvgTGPicTicks = goodTicks / goodPics;
            trainingSession.AvgTGPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(goodTicks / goodPics));

            trainingSession.AvgTBPicTicks = badTicks / badPics;
            trainingSession.AvgTBPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(badTicks / badPics));

            trainingSession.AvgTCPicTicks = correctPicsTicks / (trainingSession.NrOfGoodCorrectImages + trainingSession.NrOfBadCorrectImages);
            trainingSession.AvgTCPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(trainingSession.NrOfGoodCorrectImages + trainingSession.NrOfBadCorrectImages));

            trainingSession.AvgTWPicTicks = wrongPicsTicks / (trainingSession.NrOfGoodWrongImages + trainingSession.NrOfBadWrongImages);
            trainingSession.AvgTWPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(trainingSession.NrOfGoodWrongImages + trainingSession.NrOfBadWrongImages));

            trainingSession.AvgTGAndCPicTicks = correctAndGoodPicTicks / trainingSession.NrOfGoodCorrectImages;
            trainingSession.AvgTGAndCPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(correctAndGoodPicTicks / trainingSession.NrOfGoodCorrectImages));

            trainingSession.AvgTBAndCPicTicks = correctAndBadPicTicks / trainingSession.NrOfBadCorrectImages;
            trainingSession.AvgTBAndCPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(correctAndBadPicTicks / trainingSession.NrOfBadCorrectImages));


            trainingSession.AvgTGAndWPicTicks = wrongAndGoodPicTicks / trainingSession.NrOfGoodWrongImages;
            trainingSession.AvgTGAndWPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(correctAndGoodPicTicks / trainingSession.NrOfGoodWrongImages));

            trainingSession.AvgTBAndWPicTicks = wrongAndBadPicTicks / trainingSession.NrOfBadWrongImages;
            trainingSession.AvgTBAndWPic = stringmethods.TimeSpanToStringToMin(TimeSpan.FromTicks(correctAndBadPicTicks / trainingSession.NrOfBadWrongImages));

            trainingSessionDBHelper.AddTrainingSession(trainingSession);

            //Debug
            var debugTraining = trainingSessionDBHelper.GetLastTrainingSessions();
        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void OnDragging(object sender, DraggingCardEventArgs e)
        {
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
                    if (isUp)
                    {
                        // Picture was moved up and the type is also bad
                        if (currentPicType == PicType.Bad)
                        {
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMin(stopwatch2.Elapsed);
                            picTime.Type = PicType.Bad;
                            picTime.CorrectImage = true;
                            picTimeDBHelper.AddPicTime(picTime);                      
                            trainingSession.NrOfBadCorrectImages += 1;
                            stopwatch2.Restart();
                        }
                        // Picture was moved up but the type was good
                        else 
                        {
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMin(stopwatch2.Elapsed);
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
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMin(stopwatch2.Elapsed);
                            picTime.Type = PicType.Good;
                            picTime.CorrectImage = true;
                            picTimeDBHelper.AddPicTime(picTime);
                            trainingSession.NrOfGoodCorrectImages += 1;
                            stopwatch2.Restart();
                        }
                        // Picture was moved down but the type was bad
                        else
                        {
                            PicTime picTime = new PicTime();
                            picTime.SessionID = trainingSession.SessionId;
                            picTime.TimeTicks = stopwatch2.ElapsedTicks;
                            picTime.Time = stringmethods.TimeSpanToStringToMin(stopwatch2.Elapsed);
                            picTime.Type = PicType.Good;
                            picTime.CorrectImage = true;
                            picTimeDBHelper.AddPicTime(picTime);
                            trainingSession.NrOfBadWrongImages += 1;
                            stopwatch2.Restart();
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public async void Exit_button_Clicked(object sender, EventArgs e)
        {
            GlobalVariables.Stopwatch.Stop();
            var result = await DisplayAlert("Exit", "Do you want to quit the test?", "Yes", "No");

            if (result == true) //if yes is true\
            {
                GlobalVariables.isNavigation = false;
                await Navigation.PushAsync(new ResultsPage(trainingSession, false));

            }
            else
            {
                //Methode um Timer weiterzufuehren
                GlobalVariables.Stopwatch.Start();
            }
            return;
        }
        public async void CheckInternetConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an, um die Daten automatisch zu senden", "OK");
            }
        }
    }
}