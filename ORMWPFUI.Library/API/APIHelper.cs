using ORMWPFUI.Library.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.API
{
    public class APIHelper:IAPIHelper
    {
        private HttpClient _aAPIClient;
        private ILoggedInUserModel _loggedInUser;

        public HttpClient HttpClient
        {
            get
            {
                return _aAPIClient;
            }
        }
        public APIHelper(ILoggedInUserModel loggedInUserModel)
        {
            InitializeClient();
            _loggedInUser = loggedInUserModel;
        }
        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];
            _aAPIClient = new HttpClient();
            _aAPIClient.BaseAddress = new Uri(api);
            _aAPIClient.DefaultRequestHeaders.Accept.Clear();
            _aAPIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password",password),
            });
            using (HttpResponseMessage reponse = await _aAPIClient.PostAsync("/Token", data))
            {
                if (reponse.IsSuccessStatusCode)
                {
                    var result = await reponse.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(reponse.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            _aAPIClient.DefaultRequestHeaders.Accept.Clear();
            _aAPIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _aAPIClient.DefaultRequestHeaders.Clear();
            _aAPIClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using(HttpResponseMessage response = await _aAPIClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.Token = token;
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress= result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Id = result.Id;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
