using App1.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace App1.Helpers
{

    public class APIUserHelper
    {
        private HttpClient httpClient;

        public APIUserHelper()
        {
            httpClient = new HttpClient();
        }

        public async Task<int> SendUser(User user)
        {
            Dictionary<string, string> userDB;

            // If data protection is accepted then send all data. 
            // Otherwise only the email
            if (user.IsDataProtectionAccepted)
            {
                userDB = new Dictionary<string, string>
                {
                    { "Email", user.Email},
                    { "FirstName", user.FirstName },
                    { "LastName", user.FirstName},
                    { "Age",Convert.ToString(user.Age)}
                };
            }
            else
            {
                userDB = new Dictionary<string, string>
                {
                    { "Email", user.Email},
                    { "FirstName", null },
                    { "LastName", null},
                    { "Age",null}
                };
            }

            var isAvailable = await CrossConnectivity.Current.IsRemoteReachable(GlobalVariables.ServerIP, 443);
            if (isAvailable)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(userDB, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var serverName = GlobalVariables.ServerName + "user/create.php";
                    var message = await httpClient.PostAsync(serverName, content);
                    var response = await message.Content.ReadAsStringAsync();
                    Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                    return Convert.ToInt32(htmlAttributes["UserID"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return 0;
                }
            }
            else
            {
                // return -1 if server is not reachable
                return -1;
            }

        }

        public async Task<int> GetUserID(string userEmail)
        {
            string serverName = GlobalVariables.ServerName + "get_id.php?Email=" + userEmail;
            var respone = await httpClient.GetStringAsync(serverName);
            Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(respone);
            int returnedId = Convert.ToInt32(htmlAttributes["UserID"]);
            return returnedId;
        }

    }


}
