﻿using App1.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace App1.PopUpViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AscendingPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        bool isAscendingSelect;
        bool isDescendingSelect;

        public AscendingPopUp()
        {
            InitializeComponent();
            setIsChecked(Preferences.Get(constants.isAscending, "True"));
        }

        private void Ascending_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isAscendingSelect = e.Value;
        }

        private void Descending_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isDescendingSelect = e.Value;
        }

        private void setIsChecked(string sort)
        {
            if (sort.Equals("True"))
                IsAscending.IsChecked = true;
            else if (sort.Equals("False"))
                IsDescending.IsChecked = true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            
            if (isAscendingSelect)
            {
                Preferences.Set(constants.isAscending, isAscendingSelect.ToString());
            }
            else if (isDescendingSelect)
            {
                Preferences.Set(constants.isAscending, isAscendingSelect.ToString());
            }
            
            MessagingCenter.Send<App, string>(App.Current as App, "PopUpSort", isAscendingSelect.ToString());
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}