using App1.Helpers;
using App1.Models;
using App1.View;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataProtectionPage : ContentPage
    {
        public UserDBHelper userDBHelper = new UserDBHelper();
        bool isAccepted = false;
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
                isAccepted = true;

            }
            else
            {
                await DisplayAlert("Status", "Sie haben die Datenschutzerkärung nicht akzeptiert!", "OK");
                isAccepted = false;
            }
            
        }

        async void Continue_Clicked(object sender, System.EventArgs e)
        {
            if(isAccepted)
                await Navigation.PushAsync(new MenuPage());
            else
                await DisplayAlert("Achtung", "Bitte die Datenschutzerklärung akzeptieren", "OK");
        }

        // This prevents a user from being able to hit the back button and leave the Dataprot page to login
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}