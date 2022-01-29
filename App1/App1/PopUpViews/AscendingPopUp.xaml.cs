using App1.Models;
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
        string nagivationType;

        public AscendingPopUp(string navigationtype)
        {

            InitializeComponent();
            nagivationType = navigationtype;
            switch (navigationtype)
            {
                case constants.userPopup:
                    setIsChecked(Preferences.Get(constants.isAscending, "True"));
                    break;
                case constants.imagePopup:
                    setIsChecked(Preferences.Get(constants.isImageAscending, "True"));
                    break;
                case constants.goodimagePopup:
                    setIsChecked(Preferences.Get(constants.isGoodImageAscending, "True"));
                    break;
                case constants.badimagePopup:
                    setIsChecked(Preferences.Get(constants.isBadImageAscending, "True"));
                    break;

            }
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
            switch (nagivationType)
            {
                case constants.userPopup:
                    if (isAscendingSelect)
                    {
                        Preferences.Set(constants.isAscending, isAscendingSelect.ToString());
                    }
                    else if (isDescendingSelect)
                    {
                        Preferences.Set(constants.isAscending, isAscendingSelect.ToString());
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.userPopup, isAscendingSelect.ToString());
                    break;
                case constants.imagePopup:
                    if (isAscendingSelect)
                    {
                        Preferences.Set(constants.isImageAscending, isAscendingSelect.ToString());
                    }
                    else if (isDescendingSelect)
                    {
                        Preferences.Set(constants.isImageAscending, isAscendingSelect.ToString());
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.imagePopup, isAscendingSelect.ToString());
                    break;
                case constants.goodimagePopup:
                    if (isAscendingSelect)
                    {
                        Preferences.Set(constants.isGoodImageAscending, isAscendingSelect.ToString());
                    }
                    else if (isDescendingSelect)
                    {
                        Preferences.Set(constants.isGoodImageAscending, isAscendingSelect.ToString());
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.goodimagePopup, isAscendingSelect.ToString());
                    break;
                case constants.badimagePopup:
                    if (isAscendingSelect)
                    {
                        Preferences.Set(constants.isBadImageAscending, isAscendingSelect.ToString());
                    }
                    else if (isDescendingSelect)
                    {
                        Preferences.Set(constants.isBadImageAscending, isAscendingSelect.ToString());
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.badimagePopup, isAscendingSelect.ToString());
                    break;
            }
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}