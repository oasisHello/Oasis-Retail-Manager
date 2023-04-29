using ORMDataManager.Library.DataAccess.Models;
using ORMDataManager.Library.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<DBUserModel> GetUserById(string Id)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            var p = new { Id =Id};
            string sqlCommnd = "SELECT Id,FisrtName,LastName,EmailAddress,CreatedDate FROM [dbo].[User] WHERE Id =@Id";
            var output=sqlDataAccess.LoadData<DBUserModel,dynamic>(sqlCommnd,p,"DefaultConnection",CommandType.Text);

            return output;
        }
    }
}
