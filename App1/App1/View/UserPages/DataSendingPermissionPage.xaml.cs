using App1.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataSendingPermissionPage : ContentPage
    {
        public UserDBHelper userDBHelper = new UserDBHelper();
        public DataSendingPermissionPage()
        {
            InitializeComponent();
        }


        async void Continue_Clicked(object sender, System.EventArgs e)
        {
            userDBHelper.UpdateDataAutoSend(Switch_DataAutoSend.IsToggled);
            await Navigation.PushAsync(new MenuPage());
        }
    }
}