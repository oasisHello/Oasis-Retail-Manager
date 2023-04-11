using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUserInterface.Heplers
{
    public class APIHelper
    {
        public HttpClient APIClient { get; set; }

        public APIHelper()
        {
            InitializeClient(); 
        }
        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];
            APIClient = new HttpClient();
            APIClient.BaseAddress = new Uri(api);
            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password",password),
            });
            using (HttpResponseMessage reponse = await APIClient.PostAsync("/Token",data))
            {
                if (reponse.IsSuccessStatusCode)
                {
                    var result = await reponse.Content.ReadAsAsync<string>();
                }
            }
        }
    }
}
