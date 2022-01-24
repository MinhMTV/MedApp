
using App1.Helpers;
using App1.Models;
using App1.View;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.Extensions;
using App1.View.UserPages;
using App1.View.AdminPages;

namespace App1.View.GeneralPages
{
    public partial class WelcomePage : ContentPage
    {
        AdminDBHelper adminDBHelper;
        UserDBHelper userDBHelper;

        //TrainingSessionDBHelper trainingSessionDBHelper;
        public WelcomePage()
        {
            InitializeComponent();
            userDBHelper = new UserDBHelper();
            adminDBHelper = new AdminDBHelper();
        } 

        /* if app was closed and open again, check if a User is already logged in
         * if a user didnt log out, skip the login and go directly to training
         * we check if admin was logged in, if admin was logged in, the should be loggout because admin shouldnt be logged permanently
         */
        async void Start_Clicked(object sender, System.EventArgs e)
        {
            Console.WriteLine(Preferences.Get(constants.loginUser, "false"));
            if (adminDBHelper.IsRegisteredUserExists())  //check if there is a admin already
            {
                if (Preferences.Get(constants.loginUser, "false").Equals("false")) //check if somebody is loggin
                {
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    var logginUser = Preferences.Get(constants.loginUser, "false"); //get name of log in User

                    if (adminDBHelper.CheckUserexist(logginUser))
                    {
                        Preferences.Set(constants.loginUser, "false");
                        await Navigation.PushAsync(new LoginPage()); //if admin was loggin pls repeat login as admin shouldnt be login all the time
                    }
                    else if (userDBHelper.CheckUserexist(logginUser)) // check if user was login
                    {
                        userDBHelper.LogInUser(logginUser);           //login user automatically
                        await Navigation.PushAsync(new MenuPage());
                    }
                    else
                    {
                        throw new Exception("something went wrong..");
                    }
                }
            }
            else //if no admin is there, go to adminreg
            {
                await Navigation.PushAsync(new AdminReg());
            }
        }
    }
}
