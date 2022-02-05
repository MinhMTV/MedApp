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
    public partial class TrainingCollectionPage : ContentPage
    {
        private SessionCollectionViewModel _svm;
        public TrainingCollectionPage(User user)
        {
            InitializeComponent();
            BindingContext = _svm = new SessionCollectionViewModel(user);
        }
    }
}