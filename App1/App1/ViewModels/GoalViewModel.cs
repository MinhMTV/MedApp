using App1.Helpers;
using App1.Methods;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class GoalViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public UserDBHelper userDBHelper = new UserDBHelper();

        public TrainingSessionDBHelper TrainingSessionDBHelper = new TrainingSessionDBHelper();

        private Stringmethods stringmethods = new Stringmethods();
        public ObservableCollection<Image> cups { get; set; } = new ObservableCollection<Image>();

        public int CollectionHeight { get; set; }


        public User User { get; set; }

        //User Properties

        public string FirstName { get; set; }

        public string ElapsedTime { get; set; }

        public int TotalPictures { get; set; } = 0;

        public long TotalTime { get; set; } = 0;

        public int cmlpSessions { get; set; } 

        public int cupscount { get; set; } //nr of cups a user get

        public bool isPokal { get; set; } //is this trainingssession already his last ts where he gets the cups

        public bool isNextPokal { get; set; } //next or n number of next tsession until user gets cup

        public int countUntilPokal { get; set; } //nr of tsession to get cup

        public int defaultPokalCount { get; set; } //default nr of tsession to get a new cup


        public GoalViewModel(User obj)
        {
            InitData(obj);
        }

        private void InitData(User obj)
        {
            User = obj;
            FirstName = obj.FirstName;
            var tlist = TrainingSessionDBHelper.getCompletedTrainingSessionListbyUserAndOrder(User, false);
            cmlpSessions = tlist.Count;

            foreach (var t in tlist)
            {
                TotalTime += t.SessionTimeTicks;
                TotalPictures += t.NrOfAllImages;
            }
            ElapsedTime = stringmethods.TimeSpanToStringToHExt(TimeSpan.FromTicks(TotalTime));

            defaultPokalCount = Settings.defaultCupCount;

            cupscount = cmlpSessions / defaultPokalCount;


            if (cupscount < 7)
            {
                CollectionHeight = 60;
            } else
            {
                CollectionHeight = ((cupscount / 6)+1) * 60;
            }
            

            for (int i = 0; i < cupscount; i++)
            {
                cups.Add(new Image { Source = "pokal.png"});
            }

            if (cmlpSessions % defaultPokalCount == 0)
            {
                isPokal = true;
                isNextPokal = false;
            }
            else
            {
                countUntilPokal = defaultPokalCount - (cmlpSessions % defaultPokalCount);
                isPokal = false;
                isNextPokal = true;
            }
        }
    }
}