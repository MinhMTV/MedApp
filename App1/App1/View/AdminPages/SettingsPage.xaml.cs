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

        public int TCupAmount { get; set; }

        public bool isPic { get; set; }
        public bool isTime { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            TMin = Settings.defaultMin;
            TSec = Settings.defaultSec;
            TPicAmount = Settings.defaultPicCount;
            isPic = Settings.isPicAmount;
            isTime = Settings.isTimer;
            TCupAmount = Settings.defaultCupCount;
        }

        void OnTimeToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                isTime = true;
            }
            else
            {
                isTime = false;
            }
        }

        void OnPicToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                isPic = true;
            }
            else
            {
                isPic = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(TMin + TSec == 0 && isTime)
            {
                await DisplayAlert("Achtung", "Trainigszeit kann nicht null sein", "ok");
                return;
            } else
            {
                Settings.defaultMin = TMin;
                Settings.defaultSec = TSec;
            }
            if (TPicAmount == 0)
            {
                await DisplayAlert("Achtung", "Trainingsbilderanzahl kann nicht null sein", "ok");
                return;
            }
            else
            {
                Settings.defaultPicCount = TPicAmount;
            }

            if (TCupAmount == 0)
            {
                await DisplayAlert("Achtung", "Anzahl erreichter Trainings pro Pokal kann nicht null sein", "ok");
                return;
            }
            else
            {
                Settings.defaultCupCount = TCupAmount;
            }

            Settings.isPicAmount = isPic;
            Settings.isTimer = isTime;

            Console.WriteLine("Time" + Settings.defaultMin + Settings.defaultSec);
            Console.WriteLine("Pics" + Settings.defaultPicCount.ToString());
            await App.Current.MainPage.DisplayToastAsync("Einstellungen wurden gespeichert");
        }
    }
}