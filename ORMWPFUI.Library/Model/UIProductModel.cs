using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.Model
{
    public class UIProductModel
    {
        private int _available;
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsTaxable { get; set; }
        public int AvailableQuantity
        {
            get { return _available; }
            set { _available = value; }
        }
        public string DisplayText
        {
            get
            {
                return $"{ProductName}({AvailableQuantity})";
            }
        }
    }
}
