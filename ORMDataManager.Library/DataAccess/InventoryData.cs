using ORMDataManager.Library.Internal.DataAccess;
using ORMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<DBInventoryModel> GetAll()
        {
            SqlDataAccess dataAccess = new SqlDataAccess();
            List<DBInventoryModel> inventories = new List<DBInventoryModel>();
            string sqlCommand = "SELECT [Id],[ProductId],[Quantity],[PurchasePrice],[PurchaseDate] FROM [dbo].[Inventory]";
            inventories= dataAccess.LoadData<DBInventoryModel, dynamic>(sqlCommand, new { }, "DefaultConnection", CommandType.Text);
            return inventories;
        }

        public void SaveInventoryRecord(DBInventoryModel item)
        {
            SqlDataAccess dataAccess = new SqlDataAccess();
            string sqlCommand = "INSERT INTO [dbo].[Inventory]([ProductId],[Quantity],[PurchasePrice],[PurchaseDate]) VALUES(@ProductId,@Quantity,@PurchasePrice,@PurchaseDate)";
            dataAccess.SaveData<DBInventoryModel>(sqlCommand,item,"DefaultConnection",CommandType.Text);
        }
    }
}
