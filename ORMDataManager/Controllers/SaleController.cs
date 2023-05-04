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
    //[Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            SaleData saleData = new SaleData();
            saleData.SaveSale(sale,userId);

        }
        [Route("GetSalesReport")]
        public List<DBSaleReportModel> GetSaleReport()
        {
            SaleData saleData = new SaleData();
            return saleData.GetSaleReport();
        }
    }
}
