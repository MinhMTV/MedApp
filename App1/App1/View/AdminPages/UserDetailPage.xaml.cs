﻿using App1.Helpers;
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
		public UserDetailPage (User obj)
		{
			BindingContext = _uvm = new UserDetailViewModel(obj);
			InitializeComponent ();
		}

		public void OnDelete(System.Object sender, System.EventArgs e)
        {

        }

		public void OnEdit(System.Object sender, System.EventArgs e)
        {

        }


	}
}