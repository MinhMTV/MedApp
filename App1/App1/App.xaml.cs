using App1.Models;
using App1.View.GeneralPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App1
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;



        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new WelcomePage());
        }

        protected override void OnStart()
        {
            // On start runs when your application launches from a closed state, 
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            GlobalVariables.Stopwatch.Reset();

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            GlobalVariables.Stopwatch.Start();
        }
    }
}
