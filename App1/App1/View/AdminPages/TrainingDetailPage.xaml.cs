using App1.Models;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingDetailPage : ContentPage
    {
        TrainingStatViewModel _tvm;

        public TrainingDetailPage(TrainingSession trainingSession)
        {
            InitializeComponent();
            BindingContext = _tvm = new TrainingStatViewModel(trainingSession);
        }
    }
}