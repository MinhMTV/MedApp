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
	public partial class UserDetailPage : ContentPage
	{
		private UserDetailViewModel _uvm;
        private User user;
		public UserDetailPage (User obj)
		{
            user = obj;
            BindingContext = _uvm = new UserDetailViewModel(obj);
			InitializeComponent ();
		}

        private async void OnAlleTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingCollectionPage(user));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = _uvm = new UserDetailViewModel(user);
        }

    }
}