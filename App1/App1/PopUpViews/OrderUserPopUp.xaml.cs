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
        bool IsAlterSelect { get; set; }
        bool IsNachnameSelect { get; set; }
        bool IsVornameSelect { get; set; }
        bool IsEmailSelect { get; set; }
        bool IsUserIdSelect { get; set; }
        bool IsFirstSessionSelect { get; set; }
        bool IsLastSessionSelect { get; set; }
        bool IsSessionLastUpdatedSelect { get; set; }
        bool IsTherapieStartSelect { get; set; }
        bool IsTherapieEndSelect { get; set; }
        //kein platz im frame mehr
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

        private void Nachname_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsNachnameSelect = e.Value;
        }
        private void Vorname_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsVornameSelect = e.Value;
        }

        private void Email_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsEmailSelect = e.Value;
        }

        private void Alter_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsAlterSelect = e.Value;
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

        private void TherapieStart_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsTherapieStartSelect = e.Value;
        }

        private void TherapieEnd_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsTherapieEndSelect = e.Value;
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
                case "username":
                    IsUsername.IsChecked = true;
                    break;
                case "email":
                    IsEmail.IsChecked = true;
                    break;
                case "vorname":
                    IsVorname.IsChecked = true;
                    break;
                case "nachname":
                    IsNachname.IsChecked = true;
                    break;
                case "alter":
                    IsAlter.IsChecked = true;
                    break;
                case "userid":
                    IsUserID.IsChecked = true;
                    break;
                case "firstsession":
                    IsFirstSession.IsChecked = true;
                    break;
                case "lastsession":
                    IsLastSession.IsChecked = true;
                    break;
                case "createdat":
                    IsCreatedAt.IsChecked = true;
                    break;
                case "isSessionlastupdated":
                    IsSessionLastUpdated.IsChecked = true;
                    break;
                case "start":
                    IsTherapieStart.IsChecked = true;
                    break;
                case "end":
                    IsTherapieEnd.IsChecked = true;
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
                name = "username";
            } else if (IsAlterSelect)
            {
                name = "alter";
            }
            else if (IsNachnameSelect)
            {
                name = "nachname";
            }
            else if (IsVornameSelect)
            {
                name = "vorname";
            }
            else if (IsEmailSelect)
            {
                name = "email";
            }
            else if (IsUserIdSelect)
            {
                name = "userid";
            }
            else if (IsLastSessionSelect)
            {
                name = "lastsession";
            }
            else if (IsCreatedAtSelect)
            {
                name = "createdat";
            }
            else if (IsTherapieStartSelect)
            {
                name = "start";
            }
            else if (IsTherapieEndSelect)
            {
                name = "end";
            }
/*            else if (IsIsDataProtectionAcceptedSelect)
            {
                name = "IsDataProtectionAccepted";
            }
            else if (IsIsToDataAutoSendSelect)
            {
                name = "IsToDataAutoSend";
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