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
        public List<ProductModel> GetAll()
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            string sqlCommand = "SELECT Id,ProductName,[Description],RetailPrice,QuantityInStock FROM [dbo].[Product] ORDER BY ProductName";
            var output = sqlDataAccess.LoadData<ProductModel, dynamic>(sqlCommand, new { }, "DefaultConnection", CommandType.Text);
            return output;
        }
    }
}
