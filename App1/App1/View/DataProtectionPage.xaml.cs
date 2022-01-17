using App1.Helpers;
using App1.Models;
using App1.View;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataProtectionPage : ContentPage
    {
        public UserDBHelper userDBHelper = new UserDBHelper();
        public DataProtectionPage()
        {
            InitializeComponent();
        }


        // Perform required operation after examining e.Value
        async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                await DisplayAlert("Status", "Sie haben die Datenschutzerkärung akzeptiert!", "OK");
                //Update information

            }
            else await DisplayAlert("Status", "Sie haben die Datenschutzerkärung nicht akzeptiert!", "OK");
        }

        async void Continue_Clicked(object sender, System.EventArgs e)
        {
            userDBHelper.UpdateDataPrptectionInformation(CheckBox_IsDataProtectionAccepted.IsChecked);
            await Navigation.PushAsync(new DataSendingPermissionPage());
        }

    }
}