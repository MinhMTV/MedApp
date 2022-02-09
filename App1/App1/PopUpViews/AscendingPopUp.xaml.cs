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
                    setIsChecked(Settings.isAscending);
                    break;
                case constants.imagePopup:
                    setIsChecked(Settings.isImageAscending);
                    break;
                case constants.goodimagePopup:
                    setIsChecked(Settings.isGoodImageAscending);
                    break;
                case constants.badimagePopup:
                    setIsChecked(Settings.isBadImageAscending);
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

        private void setIsChecked(bool sort)
        {
            if (sort)
                IsAscending.IsChecked = true;
            else if (sort == false)
                IsDescending.IsChecked = true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            switch (nagivationType)
            {
                case constants.userPopup:
                    if (isAscendingSelect)
                    {
                        Settings.isAscending = isAscendingSelect;
                    }
                    else if (isDescendingSelect)
                    {
                        Settings.isAscending = isAscendingSelect;
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.userPopup, isAscendingSelect.ToString());
                    break;
                case constants.imagePopup:
                    if (isAscendingSelect)
                    {
                        Settings.isImageAscending = isAscendingSelect;
                    }
                    else if (isDescendingSelect)
                    {
                        Settings.isImageAscending = isAscendingSelect;
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.imagePopup, isAscendingSelect.ToString());
                    break;
                case constants.goodimagePopup:
                    if (isAscendingSelect)
                    {
                        Settings.isGoodImageAscending = isAscendingSelect;
                    }
                    else if (isDescendingSelect)
                    {
                        Settings.isGoodImageAscending = isAscendingSelect;
                    }
                    MessagingCenter.Send<App, string>(App.Current as App, constants.goodimagePopup, isAscendingSelect.ToString());
                    break;
                case constants.badimagePopup:
                    if (isAscendingSelect)
                    {
                        Settings.isBadImageAscending = isAscendingSelect;
                    }
                    else if (isDescendingSelect)
                    {
                        Settings.isBadImageAscending = isAscendingSelect;
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