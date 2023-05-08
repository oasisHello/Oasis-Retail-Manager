using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMDataManager.Library.Models
{
    public class DBUserRoleModel
    {
        public string Id { get; set; }
        public string EMail { get; set; }

        public Dictionary<string,string> Roles { get; set; } = new Dictionary<string,string>();
    }
}
