using CBMTraining.Models;
using CBMTraining.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CBMTraining.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoalPage : ContentPage
    {
        GoalViewModel goalViewModel;
        public GoalPage(User obj)
        {
            InitializeComponent();
            BindingContext = goalViewModel = new GoalViewModel(obj);
        }
    }
}