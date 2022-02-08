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