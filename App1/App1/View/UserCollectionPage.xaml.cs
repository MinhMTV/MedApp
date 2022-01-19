using App1.Helpers;
using App1.Models;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserCollectionPage : ContentPage
    {
        public List<User> AllUser { get; set; }
        UserDBHelper userdbhelper = new UserDBHelper();
        private UserCollectionViewModel _uvm;
        public UserCollectionPage()
        {
            BindingContext = _uvm = new UserCollectionViewModel();
            InitializeComponent();
        }

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Backdrop.Opacity == 0)
            {
                await OpenDrawer();
            }
            else
            {
                await CloseDrawer();
            }
        }
    async void ClearItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await CloseDrawer();
            UserCV.SelectedItems.Clear();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        uint duration = 100;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                Console.WriteLine($"Running: {e.TotalY}");
                if (e.TotalY > 0)
                {
                    BottomToolbar.TranslationY = openY + e.TotalY;
                }

            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                //Debug.WriteLine($"Completed: {e.TotalY}");
                if (lastPanY < 110)
                {
                    await OpenDrawer();
                }
                else
                {
                    await CloseDrawer();
                }
                isBackdropTapEnabled = true;
            }
        }

        async Task OpenDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(1, length: duration),
                BottomToolbar.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = false;
        }

        async Task CloseDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: duration),
                BottomToolbar.TranslateTo(0, 0, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = true;
        }

        void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);
        }

        void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            var selectedContact = currentSelectedContact.FirstOrDefault() as User;
            Console.WriteLine("FullName: " + selectedContact.Fullname);
            Console.WriteLine("LastSession: " + selectedContact.LastSession);
            Console.WriteLine("Email: " + selectedContact.Email);
            Console.WriteLine("Age: " + selectedContact.Age);
            Console.WriteLine("UserID: " + selectedContact.UserID);
        }
    }
}