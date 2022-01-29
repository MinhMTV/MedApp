using App1.Helpers;
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
		private UserCollectionViewModel _uvm;
		private UserDBHelper userDBHelper = new UserDBHelper();
		public UserDetailPage ()
		{
			BindingContext = _uvm = new UserCollectionViewModel();
			InitializeComponent ();
		}
	}
}