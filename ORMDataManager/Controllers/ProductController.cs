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
    [Authorize(Roles ="Programmer")]
    public class ProductController : ApiController
    {
        public List<DBProductModel> Get()
        {
            ProductData data = new ProductData();
            return data.GetAll();

        }
    }
}
