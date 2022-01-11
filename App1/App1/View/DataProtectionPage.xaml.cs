using App1.Helpers;
using App1.Models;
using App1.View;

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
            string username = GlobalVariables.CurrentLoggedInUser;
        }

        //async void Cancel_Clicked(object sender, System.EventArgs e)
        //{
        //    await DisplayAlert("Achtung", "Sie müssen die Datenschutzrichtlinien akzeptieren!", "ok");
        //}

        //async void Accept_Clicked(object sender, System.EventArgs e)
        //{
        //    //userDBHelper.UpdateDataPrptectionInformation(userDBHelper.Ge)
        //    await Navigation.PushAsync(new LoginPage());
        //}

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