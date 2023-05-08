using Microsoft.AspNet.Identity;
using ORMDataManager.Library.DataAccess;
using ORMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ORMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [Authorize(Roles ="Cashier,Admin")]
        public void Post(SaleModel sale)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            SaleData saleData = new SaleData();
            saleData.SaveSale(sale,userId);

        }
        [Authorize(Roles ="Manager,Admin")]
        [Route("GetSalesReport")]
        public List<DBSaleReportModel> GetSaleReport()
        {
            if (RequestContext.Principal.IsInRole("Manager"))
            {
                //TO-DO: do manager stuff
            }
            else if(RequestContext.Principal.IsInRole("Admin"))
            {
                //TO-DO: do admin stuff
            }
            SaleData saleData = new SaleData();
            return saleData.GetSaleReport();
        }
    }
}
