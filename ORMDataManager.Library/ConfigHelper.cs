using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ORMDataManager.Library
{
    //TODO: move this from config to API
    public class ConfigHelper
    {
        static public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];
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