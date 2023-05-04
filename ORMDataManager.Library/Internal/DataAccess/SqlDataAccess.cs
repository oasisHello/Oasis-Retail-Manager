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
    internal class SqlDataAccess:IDisposable
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public string Get(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T,U>(string sqlCommand,U parameters, string connectionStringName,CommandType commandType)
        {
            string connectionString = Get(connectionStringName);
            using(IDbConnection conn = new SqlConnection(connectionString))
            {
                List<T> rows = conn.Query<T>(sqlCommand, parameters, commandType: commandType).ToList();//Note: An extension method
                return rows;
            }
        }

        public void SaveData<T>(string sqlCommand,T parameters, string connectionStringName,CommandType type)
        {
            string connectionString = Get(connectionStringName);
            using(IDbConnection conn = new SqlConnection(connectionString))
            {
                conn.Execute(sqlCommand, parameters, commandType: type) ;
            }
        }

        //Start a transaction, open a connection
        public void StartTransaction( string connectionStringName)
        {
            string connectionString = Get(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        //Save data in Transaction
        public void SaveDataInTransaction<T>(string sqlCommand,T parameters,CommandType type)
        {
            _connection.Execute(sqlCommand, parameters, _transaction, commandType:type);
        }

        //Load data in Transaction
        public List<T> LoadDataInTransaction<T,U>(string sqlCommand,U parameters, CommandType type)
        {
            List<T> rows=_connection.Query<T>(sqlCommand, parameters, _transaction, commandType: type).ToList();
            return rows;
        }

        //Commit the transaction, close the connection
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;
            _connection?.Close();
            _connection= null;
        }

        //Rollback the transaction, close the connection
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null;
            _connection?.Close();
            _connection= null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = null;
            _connection= null;
        }
    }
}
