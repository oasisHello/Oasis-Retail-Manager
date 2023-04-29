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
    public class ProductData
    {
        public List<DBProductModel> GetAll()
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            string sqlCommand = "SELECT Id,ProductName,[Description],RetailPrice,QuantityInStock,IsTaxable FROM [dbo].[Product] ORDER BY ProductName";
            var output = sqlDataAccess.LoadData<DBProductModel, dynamic>(sqlCommand, new { }, "DefaultConnection", CommandType.Text);
            return output;
        }

        public DBProductModel GetProductById(int id)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            string sqlCommand = "SELECT Id,ProductName,[Description],RetailPrice,QuantityInStock,IsTaxable FROM [dbo].[Product] WHERE Id=@Id";
            var output = sqlDataAccess.LoadData<DBProductModel, dynamic>(sqlCommand, new {Id=id }, "DefaultConnection", CommandType.Text);
            return output.FirstOrDefault();

        }
    }
}
