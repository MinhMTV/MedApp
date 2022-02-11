using CBMTraining.Models;
using CBMTraining.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CBMTraining.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingTotalPage : ContentPage
    {
        TrainingTotalViewModel _tvm;
        public TrainingTotalPage(User user)
        {
            InitializeComponent();
            BindingContext = _tvm = new TrainingTotalViewModel(user);

        }
    }
}