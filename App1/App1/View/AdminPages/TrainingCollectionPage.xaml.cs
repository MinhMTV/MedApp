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
    public partial class TrainingCollectionPage : ContentPage
    {
        private TrainingCollectionViewModel _svm;
        public TrainingCollectionPage(User user)
        {
            InitializeComponent();
            BindingContext = _svm = new TrainingCollectionViewModel(user);
        }
    }
}