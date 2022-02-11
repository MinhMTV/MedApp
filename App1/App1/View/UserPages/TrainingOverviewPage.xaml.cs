using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.ViewModels;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;

namespace CBMTraining.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingOverviewPage : ContentPage
    {
        UserTrainingResultViewModel _uvm;
        public TrainingOverviewPage(User user)
        {
            InitializeComponent();
            BindingContext = _uvm = new UserTrainingResultViewModel(user);
        }
    }
}