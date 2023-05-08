using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ORMDataManager.Library.DataAccess;
using ORMDataManager.Library.DataAccess.Models;
using ORMDataManager.Library.Models;
using ORMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace ORMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public DBUserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetUsersRole")]
        public List<DBUserRoleModel> GetUsersRole()
        {
            List<DBUserRoleModel> userRoleModels = new List<DBUserRoleModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    var u = new DBUserRoleModel
                    {
                        Id = user.Id,
                        EMail = user.Email
                    };
                    foreach (var role in user.Roles)
                    {
                        u.Roles.Add(role.RoleId, roles.Where(x => x.Id == role.RoleId).First().Name);
                    }
                    userRoleModels.Add(u);
                }
            }
            return userRoleModels;
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string,string> GetAllRoles()
        {
            using(var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToDictionary(x=>x.Id,x=>x.Name);
                return roles;
            }
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public void AddRole(DBUserRolePairModel pair)
        {
            using(var  context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(pair.UserId,pair.RoleName);
            }
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public void RemoveRole(DBUserRolePairModel pair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.RemoveFromRole(pair.UserId, pair.RoleName);
            }
        }

    }
}
