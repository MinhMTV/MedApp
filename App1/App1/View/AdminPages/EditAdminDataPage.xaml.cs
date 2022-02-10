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
    public partial class EditAdminDataPage : ContentPage
    {
        private AdminEditViewModel _uvm;
        public EditAdminDataPage(Admin obj)
        {
            InitializeComponent();
            BindingContext = _uvm = new AdminEditViewModel(obj);
        }
    }
}