using App1.Helpers;
using App1.Models;
using Microcharts;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;

namespace App1.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class APITest : ContentPage
    {

        HttpClient httpClient = new HttpClient();
        TrainingSessionDBHelper trainingSessionDBHelper = new TrainingSessionDBHelper();


        public APITest()
        {
            InitializeComponent();
            CheckInternetConnection();

            CheckServerAvailability();
            //var isReachable = await CrossConnectivity.Current.IsReachable("http://192.168.178.33/Rest_API/api/user/create.php", 5000)


            DateTime dateTime = DateTime.Today.Date;
            List<TrainingSession> listTS = trainingSessionDBHelper.GetLastTwoTrainingSessions();

            int totalPicuresCurrent = listTS.ElementAt(0).NrOfGoodCorrectImages + listTS.ElementAt(0).NrOfGoodWrongImages +
                                        listTS.ElementAt(0).NrOfBadCorrectImages + listTS.ElementAt(0).NrOfBadWrongImages;

            int totalPicuresLast = listTS.ElementAt(1).NrOfGoodCorrectImages + listTS.ElementAt(1).NrOfGoodWrongImages +
                                    listTS.ElementAt(1).NrOfBadCorrectImages + listTS.ElementAt(1).NrOfBadWrongImages;

            int trainingTimeCurrent = Convert.ToInt32(listTS.ElementAt(0).ElapsedTime.Substring(0, 2)) * 60
                                       + Convert.ToInt32(listTS.ElementAt(0).ElapsedTime.Substring(3, 2));

            int trainingTimeLast = Convert.ToInt32(listTS.ElementAt(1).ElapsedTime.Substring(0, 2)) * 60
                                       + Convert.ToInt32(listTS.ElementAt(1).ElapsedTime.Substring(3, 2));




            List<Entry> entries1 = new List<Entry>
            {
                 new Entry(totalPicuresCurrent)
                {
                    Label = "Aktuell",
                    ValueLabel = totalPicuresCurrent.ToString(),
                    Color = SKColor.Parse("#00ff00")
                },
                  new Entry(totalPicuresLast)
                {
                    Label = "Letze",
                    ValueLabel = totalPicuresLast.ToString(),
                    Color = SKColor.Parse("#ff0000")
                }

            };

            List<Entry> entries2 = new List<Entry>
            {
                 new Entry(trainingTimeCurrent)
                {
                    Label = "Aktuell",
                    ValueLabel = listTS.ElementAt(0).ElapsedTime + "s",
                    Color = SKColor.Parse("#00ff00")
                },
                  new Entry(trainingTimeLast)
                {
                    Label = "Letze",
                    ValueLabel = listTS.ElementAt(1).ElapsedTime + "s",
                    Color = SKColor.Parse("#ff0000")
                }

            };

            List<Entry> entries3 = new List<Entry>
            {
                 new Entry( 3)
                {
                    Label = "Aktuell",
                    ValueLabel = "3s",
                    Color = SKColor.Parse("#00ff00")
                },
                  new Entry(41)
                {
                    Label = "Letze",
                    ValueLabel = "41s",
                    Color = SKColor.Parse("#ff0000")
                }
            };

            //List<Entry> entries3 = new List<Entry>
            //{
            //     new Entry( (float) listTS.ElementAt(0).AverageReactionTime )
            //    {
            //        Label = "Aktuell",
            //        ValueLabel = listTS.ElementAt(0).AverageReactionTime.ToString(),
            //        Color = SKColor.Parse("#00ff00")
            //    },
            //      new Entry( (float) listTS.ElementAt(1).AverageReactionTime )
            //    {
            //        Label = "Letze",
            //        ValueLabel = listTS.ElementAt(1).AverageReactionTime.ToString(),
            //        Color = SKColor.Parse("#ff0000")
            //    }
            //};

            PictureChart.Chart = new BarChart() { Entries = entries1 };
            PictureChart.Chart.LabelTextSize = 40;
            PictureChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");


            TimeChart.Chart = new BarChart() { Entries = entries2 };
            TimeChart.Chart.LabelTextSize = 40;
            TimeChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");

            ReactionTimeChart.Chart = new BarChart() { Entries = entries3 };
            ReactionTimeChart.Chart.LabelTextSize = 40;
            ReactionTimeChart.Chart.BackgroundColor = SKColor.Parse("#f5f5f5");
        }


        public async Task<int> GetUserID()
        {
            var httpsResponse = await httpClient.GetStringAsync("");
            return 0;
        }
        public async void GetUser()
        {
            var serverurl = GlobalVariables.ServerName + "user/read_single.php?Email=Max@test.com";
            var respone = await httpClient.GetStringAsync(serverurl);
            Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(respone);
            Console.WriteLine(htmlAttributes["patient_id"]);


        }

        public async void SendUser()
        {
            Dictionary<string, string> userDB = new Dictionary<string, string>
            {
                { "Email", "new@email.com"},
                { "FirstName", "API" },
                { "LastName", "Test" },
                { "Age","27" }
            };

            //string jsonData = @"{""Email"" : ""1@email.com"", ""FirstName"" : ""API"", ""LastName"": ""Test"", ""Age"":""27""}";

            //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string json = JsonConvert.SerializeObject(userDB, Formatting.Indented);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var serverurl = GlobalVariables.ServerName + "Rest_API/api/user/create.php";
            var message = await httpClient.PostAsync("serverurl", content);


            if (message.IsSuccessStatusCode)
            {
                await DisplayAlert("Connection!", "Benutzer eingetragen", "OK");
            }
        }

        private void CheckInternetConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Kein Internet", "Bitte schalten Sie das Internet an, um die Daten automatisch zu senden", "OK");
            }
        }

        public async void CheckServerAvailability()
        {
            if (await CrossConnectivity.Current.IsRemoteReachable(GlobalVariables.ServerIP, 443))
            {

            }
            else
            {
                await DisplayAlert("Connection Failed!", "Server is not reachable", "Ok");
            }
        }


    }
}