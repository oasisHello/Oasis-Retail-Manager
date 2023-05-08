using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.Model
{
    public class UIUserRoleModel
    {
        public string Id { get; set; }
        public string EMail { get; set; }

        public Dictionary<string,string> Roles { get; set; } = new Dictionary<string,string>();

        public string DisplayText
        {
            get
            {
                return string.Join(", ", Roles.Select(x=>x.Value));
            }
        }
    }
}
