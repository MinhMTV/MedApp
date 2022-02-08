using App1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage, INotifyPropertyChanged
    {
        public int TMin { get; set; }
        public int TSec { get; set; }
        public int TPicAmount { get; set; }

        public bool isPic { get; set; }
        public bool isTime { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            TMin = GlobalVariables.defaultMin;
            TSec = GlobalVariables.defaultSec;
            TPicAmount = GlobalVariables.defaultPicCount;
            isPic = GlobalVariables.isPicAmount;
            isTime = GlobalVariables.isTimer;
        }

        void OnTimeToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                isTime = true;
                GlobalVariables.isTimer = true;
            }
            else
            {
                isTime = false; 
                GlobalVariables.isTimer = false;
            }
        }

        void OnPicToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                isPic = true;
                GlobalVariables.isPicAmount = true;
            }
            else
            {
                isPic = false;
                GlobalVariables.isPicAmount = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            GlobalVariables.defaultMin = TMin;
            GlobalVariables.defaultSec = TSec;
            GlobalVariables.defaultPicCount = TPicAmount;
            GlobalVariables.isPicAmount = isPic;
            GlobalVariables.isTimer = isTime;

            Console.WriteLine("Time" + GlobalVariables.defaultMin + GlobalVariables.defaultSec);
            await App.Current.MainPage.DisplayToastAsync("Einstellungen wurden gespeichert");
        }
    }
}