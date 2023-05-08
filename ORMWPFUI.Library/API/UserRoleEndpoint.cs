using ORMWPFUI.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.API
{
    public class UserRoleEndpoint :IUserRoleEndpoint
    {
        private IAPIHelper _anAPIHelper;

        public UserRoleEndpoint(IAPIHelper apiHelper)
        {
            _anAPIHelper = apiHelper;

        }

        public async Task<List<UIUserRoleModel>> GetUserRoles()
        {
            using (HttpResponseMessage response = await _anAPIHelper.HttpClient.GetAsync("/api/User/Admin/GetUsersRole"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UIUserRoleModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }

        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            using (HttpResponseMessage response = await _anAPIHelper.HttpClient.GetAsync("/api/User/Admin/GetAllRoles"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task AddRole(UIUserRolePairModel pair)
        {
            using (HttpResponseMessage response = await _anAPIHelper.HttpClient.PostAsJsonAsync("api/User/Admin/AddRole", pair))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task RemoveRole(string userId,string roleName)
        {
            using (HttpResponseMessage response = await _anAPIHelper.HttpClient.PostAsJsonAsync("api/User/Admin/RemoveRole", new { userId,roleName}))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
