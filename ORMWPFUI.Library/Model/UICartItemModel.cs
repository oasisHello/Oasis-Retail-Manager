using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.Model
{
    public class UICartItemModel
    {
        public UIProductModel Product { get; set; }
        public int QuantityInCart { get; set; }

        public string DisplayText
        {
            get
            {
                return $"{Product.ProductName}({QuantityInCart})";
            }
        }
        public override string ToString()
        {
            return $"{Product.Id}";
        }

    }
}
