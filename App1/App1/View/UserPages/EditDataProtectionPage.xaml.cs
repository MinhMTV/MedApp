using App1.Helpers;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDataProtectionPage : ContentPage
    {
        public UserDBHelper userDBHelper = new UserDBHelper();
        public EditDataProtectionPage()
        {
            InitializeComponent();
        }
        async void Continue_Clicked(object sender, System.EventArgs e)
        {
            userDBHelper.UpdateDataPrptectionInformation(CheckBox_IsDataProtectionAccepted.IsChecked);
            await this.DisplayToastAsync("Einstellung wurde gespeichert");
        }
    }
}