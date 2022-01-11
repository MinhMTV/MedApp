
using App1.Helpers;
using App1.Models;
using App1.View;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        UserDBHelper userDBHelper;
        //TrainingSessionDBHelper trainingSessionDBHelper;
        public MainPage()
        {
            InitializeComponent();
            userDBHelper = new UserDBHelper();
            //trainingSessionDBHelper = new TrainingSessionDBHelper();
            //userDBHelper.DeleteAllUser();
            //trainingSessionDBHelper.DeleteAllTSession();
        }

        async void Start_Clicked(object sender, System.EventArgs e)
        {   //
            if (userDBHelper.IsRegisteredUserExists())
            {
                if (userDBHelper.IsLoggedInUserExists())
                {
                    string userName = userDBHelper.GetLoggedInUserName();
                    GlobalVariables.CurrentLoggedInUser = userName;
                    GlobalVariables.CurrentLoggedInUserID = userDBHelper.GetUserID();
                    await Navigation.PushAsync(new MenuPage());
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new Registraion());
            }

            //// Get current logged in user
            //string userName = userDBHelper.GetLoggedInUserName();

            //// If there is no logged in user, force user to loging
            //if(string.IsNullOrEmpty(userName))
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}
            //// Else store the username and user id in the global varriables for later use
            //// and dircet user to the demo page.
            //else
            //{
            //    GlobalVariables.CurrentLoggedInUser = userName;
            //    GlobalVariables.CurrentLoggedInUserID = userDBHelper.GetUserID();
            //    await Navigation.PushAsync(new MenuPage());
            //}

        }
    }
}
