using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.Model
{
    public class UISaleModel
    {
        public List<UISaleDetailModel> SaleDetails { get; set; } = new List<UISaleDetailModel>();
    }
}
