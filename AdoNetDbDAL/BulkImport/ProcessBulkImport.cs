using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDbDAL.BulkImport
{
    public static class ProcessBulkImport
    {
        private static SqlConnection _connection = null;
        private static readonly string _connectionString =
             @"Data Source = localhost;Integrated Security=true;Initial Catalog=adonetdb";
        
        private static void OpenConnection()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        private static void CloseConnection()
        {
            if(_connection?.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }

        public static void ExecuteBulkImport<T>(IEnumerable<T> records, string tableName)
        {
            OpenConnection();

            using (SqlConnection conn = _connection)
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = tableName
                };

                var datareader = new MyDataReader<T>(records.ToList());

                try
                {
                    bulkCopy.WriteToServer(datareader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    CloseConnection();
                }
            }
        }
    }
}
