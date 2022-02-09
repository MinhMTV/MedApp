using App1.Helpers;
using App1.Models;
using App1.View.GeneralPages;
using App1.View.UserPages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App1
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;
        private AdminDBHelper adminDBHelper = new AdminDBHelper();
        private UserDBHelper userDBHelper = new UserDBHelper();



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
            if (GlobalVariables.isTraining)
            {
                GlobalVariables.Stopwatch.Stop();
            }        
       }

        protected override async void OnResume()
        {
            //check if lastpage was TrainingsPage, if yes than continue Stopwatch, if not, than check if user was logged in and return to mainmenu or to login, if admin
            if (GlobalVariables.isTraining)
            {
                var answer = await App.Current.MainPage.DisplayAlert("Achtung", "Training fortsetzen?", "Ja", "No");
                if (answer)
                {
                    GlobalVariables.Stopwatch.Start();
                }
                else
                {
                    GlobalVariables.Stopwatch.Reset();
                    await App.Current.MainPage.Navigation.PushAsync(new MenuPage());
                }
            }
            else if (GlobalVariables.isGallery)
            {
                return;
            }
            else
            {
                if (adminDBHelper.IsRegisteredUserExists())  //check if there is a admin already
                {
                    if (Settings.loginUser == "false") //check if somebody is loggin
                    {
                        GlobalVariables.isTraining = false;
                        await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        var logginUser = Settings.loginUser; //get name of log in User

                        if (adminDBHelper.CheckUserexist(logginUser))
                        {
                            Settings.loginUser = "false";
                            GlobalVariables.isTraining = false;
                            await App.Current.MainPage.Navigation.PushAsync(new LoginPage()); //if admin was loggin pls repeat login as admin shouldnt be login all the time
                        }
                        else if (userDBHelper.CheckUserexist(logginUser)) // check if user was login
                        {
                            
                                userDBHelper.LogInUser(logginUser);           //login user automatically
                                GlobalVariables.isTraining = false;
                            if (userDBHelper.GetLoggedUser().isAskDataProtec == false)
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new DataProtectionPage());
                            }
                            else
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new MenuPage());
                            }

                            
                        }
                        else
                        {
                            throw new Exception("something went wrong..");
                        }
                    }
                }
            }
        }
    }
}
