using App1.Helpers;
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
	public partial class UserDetailPage : ContentPage
	{
		private UserDetailViewModel _uvm;
		private UserDBHelper userDBHelper = new UserDBHelper();
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