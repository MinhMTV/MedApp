
using App1.Helpers;
using App1.Models;
using App1.View;
using System;
using Xamarin.Forms;

namespace App1
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

            //trainingSessionDBHelper = new TrainingSessionDBHelper();
            //userDBHelper.DeleteAllUser();
            //trainingSessionDBHelper.DeleteAllTSession();
        }

        /* if app was closed and open again, check if a User is already logged in
         * if a user didnt log out, skip the login and go directly to training
         * we check if admin was logged in, if admin was logged in, the should be loggout because admin shouldnt be logged permanently
         */
        async void Start_Clicked(object sender, System.EventArgs e)
        {
            if (adminDBHelper.IsRegisteredUserExists())  //check if there is a admin already
            {
                if (adminDBHelper.IsLoggedInUserExists()) //check if admin was logged in last session
                {
                    Admin admin = adminDBHelper.GetUser();
                    admin.IsUserLoggedIn = false;
                    await Navigation.PushAsync(new LoginPage());
                }
                else if (userDBHelper.IsLoggedInUserExists()) //check if a user was logged in last session (dont check if there is a user in db, because atleast a admin should be there
                {
                    string userName = userDBHelper.GetLoggedInUserName();
                    GlobalVariables.CurrentLoggedInUser = userName;
                    GlobalVariables.CurrentLoggedInUserID = userDBHelper.GetUserID();
                    await Navigation.PushAsync(new MenuPage());
                }
            }
            else //if no admin is there, go to adminreg
            {
                await Navigation.PushAsync(new AdminReg());
            }
        }
    }
}
