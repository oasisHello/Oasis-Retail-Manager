using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.Helper
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];//Question:The config file does not in this assembly,
                                                                          //and this assembly does not reference the assembly
                                                                          //that has the config file.
                                                                          //How can it access the config?
            bool flag = Decimal.TryParse(rateText, out decimal rate);
            if (flag)
            {
                return rate;
            }
            else
            {
                throw new ConfigurationErrorsException("The tax rate is not set properly");
            }
        }
    }
}
