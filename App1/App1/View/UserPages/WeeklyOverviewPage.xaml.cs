﻿using App1.Extensions;
using App1.Helpers;
using App1.Models;
using App1.ViewModels;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeeklyOverviewPage : ContentPage
    {
        UserTrainingWeekViewModel _utvm;
        private User user;
        public WeeklyOverviewPage(User obj)
        {
            InitializeComponent();
            DateTime dateTime = DateTime.Today.Date;
            user = obj;
            BindingContext = _utvm = new UserTrainingWeekViewModel(user, DateTime.Now.StartOfWeek(DayOfWeek.Monday), 0);
        }

        void OnLast(object sender, EventArgs args)
        {
            var setdate = _utvm.weekstart;
            setdate = setdate.AddDays(-7);
            var setChart = _utvm.swipedir;
            BindingContext = _utvm = new UserTrainingWeekViewModel(user, setdate, setChart);
        }

        void OnNext(object sender, EventArgs args)
        {
            var setdate = _utvm.weekstart;
            setdate = setdate.AddDays(7);
            var setChart = _utvm.swipedir;
            BindingContext = _utvm = new UserTrainingWeekViewModel(user, setdate, setChart);

            Console.WriteLine(setdate.ToString());
        }
    }
}
