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
    public partial class OrderUserPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        bool IsCreatedAtSelect { get; set; }
        bool IsUsernameSelect { get; set; }
        bool IsAgeSelect { get; set; }
        bool IsLastnameSelect { get; set; }
        bool IsFirstnameSelect { get; set; }
        bool IsEmailSelect { get; set; }
        bool IsUserIdSelect { get; set; }
        bool IsFirstSessionSelect { get; set; }
        bool IsLastSessionSelect { get; set; }
        bool IsSessionLastUpdatedSelect { get; set; }
        bool IsTherapyStartSelect { get; set; }
        bool IsTherapyEndSelect { get; set; }
        //no place in frame 
/*        bool IsIsToDataAutoSendSelect { get; set; }
        bool IsIsDataProtectionAcceptedSelect { get; set; }*/
        public OrderUserPopUp()
        {
            InitializeComponent();
            BindingContext = this;
            setIsChecked(Preferences.Get(constants.OrderBy,""));
        }
        private void CreatedAt_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsCreatedAtSelect = e.Value;
        }
        private void Username_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsUsernameSelect = e.Value;
        }

        private void Lastname_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsLastnameSelect = e.Value;
        }
        private void Firstname_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsFirstnameSelect = e.Value;
        }

        private void Email_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsEmailSelect = e.Value;
        }

        private void Age_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsAgeSelect = e.Value;
        }


        private void UserID_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsUserIdSelect = e.Value;
        }
        private void FirstSession_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsFirstSessionSelect = e.Value;
        }
        private void LastSession_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsLastSessionSelect = e.Value;
        }

        private void SessionLastUpdated_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsSessionLastUpdatedSelect = e.Value;
        }

        private void TherapyStart_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsTherapyStartSelect = e.Value;
        }

        private void TherapyEnd_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsTherapyEndSelect = e.Value;
        }

/*        private void IsDataProtectionAccepted_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsIsDataProtectionAcceptedSelect = e.Value;
        }

        private void IsToDataAutoSend_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsIsToDataAutoSendSelect = e.Value;
        }*/

        public void setIsChecked(String name)
        {
            switch (name.ToLower())
            {
                case constants.username:
                    IsUsername.IsChecked = true;
                    break;
                case constants.email:
                    IsEmail.IsChecked = true;
                    break;
                case constants.firstname:
                    IsFirstname.IsChecked = true;
                    break;
                case constants.lastname:
                    IsLastname.IsChecked = true;
                    break;
                case constants.age:
                    IsAge.IsChecked = true;
                    break;
                case constants.userid:
                    IsUserID.IsChecked = true;
                    break;
                case constants.firstsession:
                    IsFirstSession.IsChecked = true;
                    break;
                case constants.lastsession:
                    IsLastSession.IsChecked = true;
                    break;
                case constants.createdat:
                    IsCreatedAt.IsChecked = true;
                    break;
                case constants.sessionlastupdated:
                    IsSessionLastUpdated.IsChecked = true;
                    break;
                case constants.start:
                    IsTherapyStart.IsChecked = true;
                    break;
                case constants.end:
                    IsTherapyEnd.IsChecked = true;
                    break;
                /*                case "IsDataProtectionAccepted":
                                    IsIsDataProtectionAccepted.IsChecked = true;
                                    break;
                                case "IsToDataAutoSend":
                                    IsIsToDataAutoSend.IsChecked = true;
                                    break;*/
                default: // nothing isChecked
                    break;

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            String name = "";
            if(IsUsernameSelect)
            {
                name = constants.username;
            } else if (IsAgeSelect)
            {
                name = constants.age;
            }
            else if (IsLastnameSelect)
            {
                name = constants.lastname;
            }
            else if (IsFirstnameSelect)
            {
                name = constants.firstname;
            }
            else if (IsEmailSelect)
            {
                name = constants.email;
            }
            else if (IsUserIdSelect)
            {
                name = constants.userid;
            }
            else if (IsFirstSessionSelect)
            {
                name = constants.firstsession;
            }
            else if (IsLastSessionSelect)
            {
                name = constants.lastsession;
            }
            else if(IsSessionLastUpdatedSelect) {
                name = constants.sessionlastupdated;
            }
            else if (IsCreatedAtSelect)
            {
                name = constants.createdat;
            }
            else if (IsTherapyStartSelect)
            {
                name = constants.start;
            }
            else if (IsTherapyEndSelect)
            {
                name = constants.end;
            }
/*            else if (IsIsDataProtectionAcceptedSelect)
            {
                name = constants.IsDataProtectionAccepted;
            }
            else if (IsIsToDataAutoSendSelect)
            {
                name = constants.IsToDataAutoSend;
            }*/
            else
            {
                
            }
            Preferences.Set(constants.OrderBy, name);
            MessagingCenter.Send<App, string>(App.Current as App, "PopUpOrder", name);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return true;
        }
    }
}