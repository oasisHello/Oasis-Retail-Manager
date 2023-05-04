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
    public class InventoryController : ApiController
    {
        [Authorize(Roles ="Manager,Admin")]
        public List<DBInventoryModel> Get()
        {
            InventoryData inventory = new InventoryData();
            return inventory.GetAll();
        }
        [Authorize(Roles ="Admin")]
        public void Post(DBInventoryModel item)
        {
            InventoryData inventory = new InventoryData();
            inventory.SaveInventoryRecord(item);
        }
    }
}
