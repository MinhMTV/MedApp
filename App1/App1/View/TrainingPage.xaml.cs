using App1.Models;
using App1.ViewModels;
using MLToolkit.Forms.SwipeCardView;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
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
        int indexer = 0;
        public TrainingPage()
        {
            this.InitializeComponent();
            this.BindingContext = new SwipeViewModel();
            SwipeCardView.Dragging += OnDragging;
            trainingSession = new TrainingSession();
            newCollection = (this.BindingContext as SwipeViewModel).Pictures;


            if (!GlobalVariables.Stopwatch.IsRunning)
            {
                GlobalVariables.Stopwatch.Start();
            }

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                // Logic for logging out if the device is inactive for a period of time.

                if (GlobalVariables.Stopwatch.IsRunning && GlobalVariables.Stopwatch.Elapsed.Minutes >= defaultTimespan)
                {
                    //prepare to perform your data pull here as we have hit the 5 minute mark  Gerade ist 1 Minute eingestellt: s. defaultTimespan 
                    GlobalVariables.Stopwatch.Stop();

                    Navigation.PushAsync(new ResultsPage(trainingSession, true));
                }
                // Always return true as to keep our device timer running.
                return true;
            });
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

                        GlobalVariables.PicturesWrong += 1;
                        // Picture was moved up and the type is also bad
                        if (currentPicType == PicType.Bad)
                        {
                            trainingSession.NrOfBadCorrectImages += 1;
                        }
                        // Picture was moved up but the type was good
                        else { trainingSession.NrOfGoodWrongImages += 1; }

                    }
                    else
                    {
                        GlobalVariables.PicturesRight += 1;
                        // Picture was moved down and the type is also good
                        if (currentPicType == PicType.Good)
                        {
                            trainingSession.NrOfGoodCorrectImages += 1;
                        }
                        // Picture was moved down but the type was bad
                        else { trainingSession.NrOfBadWrongImages += 1; }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public async void Exit_button_Clicked(object sender, EventArgs e)
        {
            GlobalVariables.Stopwatch.Stop();
            var result = await DisplayAlert("Exit", "Do you want to end the test?", "Yes", "No");

            if (result == true) //if yes is true\
            {
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