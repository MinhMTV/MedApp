using CBMTraining.Helpers;
using CBMTraining.Models;
using CBMTraining.View;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CBMTraining.View.UserPages
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
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                //Update information
                isAccepted = true;

            }
            else 
            {
                isAccepted = false;
            }
            
        }

        async void Continue_Clicked(object sender, System.EventArgs e)
        {
            if (isAccepted)
            {
                var user = userDBHelper.GetLoggedUser();
                user.isAskDataProtec = true;
                userDBHelper.UpdateUser(user);
                await Navigation.PushAsync(new MenuPage());
            }  
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