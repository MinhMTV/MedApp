using CBMTraining.Extensions;
using CBMTraining.Helpers;
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
    public partial class TrainingWeekPage : ContentPage
    {
        TrainingWeekViewModel _tvm;
        private User user;
        public TrainingWeekPage(User obj)
        {
            InitializeComponent();
            user = obj;
            BindingContext = _tvm = new TrainingWeekViewModel(user, DateTime.Now.StartOfWeek(DayOfWeek.Monday),0);
        }

         void OnLast(object sender, EventArgs args)
        {
            var setdate = _tvm.weekstart;
            setdate = setdate.AddDays(-7);
            var setChart = _tvm.swipedir;
            BindingContext = _tvm = new TrainingWeekViewModel(user, setdate, setChart);
        }

        void OnNext(object sender, EventArgs args)
        {
            var setdate = _tvm.weekstart;
            setdate = setdate.AddDays(7);
            var setChart = _tvm.swipedir;
            BindingContext = _tvm = new TrainingWeekViewModel(user, setdate, setChart);

            Console.WriteLine(setdate.ToString());
        }
    }
}