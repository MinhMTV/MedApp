using App1.ViewModels;
using Plugin.LocalNotifications;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.GeneralPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocalNotificationPage : ContentPage
    {
        public LocalNotificationPage()
        {
            InitializeComponent();
            BindingContext = new LocalNotificationPageViewModel();
        }

        private void Store_Clicked(object sender, EventArgs e)
        {
            CrossLocalNotifications.Current.Show("Test", Editor_Text.Text, 101, DateTime.Now.AddSeconds(5));
        }
    }
}