using App1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App1.Helpers
{
    public class APITSHelper
    {
        private HttpClient httpClient;
        public APITSHelper()
        {
            httpClient = new HttpClient();
        }

        public async Task<bool> SendTrainingSession(TrainingSession trainingSession)
        {
            Dictionary<string, string> trainingSessionDB;
            trainingSessionDB = new Dictionary<string, string>
            {
                { "SessionDate",trainingSession.SessionDate.Date.ToString() } ,
                { "PatientId", trainingSession.PatientId.ToString()},
                { "NrOfGoodCorrectImages", trainingSession.NrOfGoodCorrectImages.ToString() },
                { "NrOfGoodWrongImages", trainingSession.NrOfGoodWrongImages.ToString() },
                { "NrOfBadCorrectImages", trainingSession.NrOfBadCorrectImages.ToString() },
                { "NrOfBadWrongImages", trainingSession.NrOfBadWrongImages.ToString()},
                { "ElapsedTime", trainingSession.ElapsedTime.ToString() },
                { "IsTrainingCompleted", (trainingSession.IsTrainingCompleted ==true)? "1":"0" },
                { "IsDataSent", "0"}
            };
            try
            {
                var jsonTS = JsonConvert.SerializeObject(trainingSessionDB, Formatting.Indented);
                var contentTS = new StringContent(jsonTS, Encoding.UTF8, "application/json");
                var serverName = GlobalVariables.ServerName + "trainingsession/create.php";
                await httpClient.PostAsync(serverName, contentTS);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }

        }
    }
}
