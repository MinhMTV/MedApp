using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DemoPage : ContentPage
    {


        public DemoPage()
        {
            InitializeComponent();
        }

        private void CheckInternetConnectionAvailability()
        {
            // Force user to get connected to the internet to complete the registration process
            while (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an!", "OK");
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TrainingPage());
            //GlobalVariables.Stopwatch.Start();
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            while ((Connectivity.NetworkAccess != NetworkAccess.Internet))
            {
                await DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an!", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

    }
}