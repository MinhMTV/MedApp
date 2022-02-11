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
    public partial class EditUserData : ContentPage
    {

        private UserEditViewModel _uvm;
        public EditUserData(User obj)
        {
            InitializeComponent();
            BindingContext = _uvm = new UserEditViewModel(obj);
        }




    }
}