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
    public partial class EditDataPage : ContentPage
    {

        private UserEditViewModel _uvm;
        public EditDataPage(User obj)
        {
            InitializeComponent();
            BindingContext = _uvm = new UserEditViewModel(obj);
        }
    }
}