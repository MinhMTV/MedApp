using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExplainPage : ContentPage
    {

        
        public ExplainPage()
        {
            InitializeComponent();
        }


        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TrainingPage());

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}