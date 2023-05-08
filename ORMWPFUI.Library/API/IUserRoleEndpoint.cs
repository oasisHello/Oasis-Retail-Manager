using ORMWPFUI.Library.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.API
{
    public interface IUserRoleEndpoint
    {
        Task AddRole(UIUserRolePairModel pair);
        Task<Dictionary<string, string>> GetAllRoles();
        Task<List<UIUserRoleModel>> GetUserRoles();
        Task RemoveRole(string userId,string roleName);
    }
}