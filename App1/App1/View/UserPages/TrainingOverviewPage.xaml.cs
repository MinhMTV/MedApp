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