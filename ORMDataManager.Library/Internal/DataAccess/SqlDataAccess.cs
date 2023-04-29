using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string Get(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T,U>(string sqlCommand,U parameters, string connectionStringName,CommandType commandType)
        {
            string connectionString = Get(connectionStringName);
            using(IDbConnection conn = new SqlConnection(connectionString))
            {
                List<T> rows = conn.Query<T>(sqlCommand, parameters, 
                    commandType: commandType).ToList();//Note: An extension method
                return rows;
            }
        }
        public void SaveData<T>(string sqlCommand,T parameters, string connectionStringName,CommandType commandType)
        {
            string connectionString = Get(connectionStringName);
            using(IDbConnection conn = new SqlConnection(connectionString))
            {
                conn.Execute(sqlCommand, parameters, commandType: commandType) ;
            }
        }

        internal void SaveData<T>(string sqlCommand, object dBSaleModel, string v, CommandType text)
        {
            throw new NotImplementedException();
        }
    }
}
