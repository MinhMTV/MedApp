using App1.Helpers;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class TrainingStatViewModel
    {
        private TrainingSessionDBHelper trainingSession = new TrainingSessionDBHelper();

        private ObservableCollection<TrainingSession> _tsession;

        public ObservableCollection<TrainingSession> TSession { get => _tsession; set => _tsession = value; }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public Command<TrainingSession> PressedCommand { get; private set; }

        public UserDBHelper userDBHelper = new UserDBHelper();

        //Training Properties

        public TrainingSession tsession { get; set; }

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
        public string ElapsedTime { get; set; }

        public string AvgT { get; set; }

        public string AvgTGPic { get; set; }

        public string AvgTBPic { get; set; }

        public string AvgTCPic { get; set; }

        public string AvgTWPic { get; set; }


        public TrainingStatViewModel(TrainingSession obj)
        {
            tsession = obj;
            InitData(tsession);
        }

        private async void OnPressed(TrainingSession obj)
        {
            await App.Current.MainPage.DisplayToastAsync("Navigiere zu Trainingssession");

            //          await App.Current.MainPage.Navigation.PushAsync(new TrainingSessionDetail(obj));
        }


        private void InitData(TrainingSession obj)
        {
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
            ElapsedTime = obj.ElapsedTime;

            AvgT = obj.AvgT;

            AvgTGPic = obj.AvgTGPic;

            AvgTBPic = obj.AvgTBPic;

            AvgTCPic = obj.AvgTCPic;

            AvgTWPic = obj.AvgTWPic;
    }
    }
}

