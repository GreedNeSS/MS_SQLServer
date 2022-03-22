using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace StoredProcedure
{
    public enum ExecuteType
    {
        ExecuteReaderAsync,
        ExecuteNonQueryAsync,
        ExecuteScalarAsync,
    }

    internal static class StoredProcedureClass
    {
        public static async Task CreateProcedureAsync(string connectionString, string sqlExpression)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    Console.WriteLine("Процедура добавлена в базу данных!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static async Task<object> ExecutingProcedureAsync(string connectionString,
            string procedureName, ExecuteType executeType, Dictionary<string, string> parameters = null)
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            if (parameters is not null)
            {
                foreach (var (key, value) in parameters)
                {
                    command.Parameters.AddWithValue(key, value);
                }
            }

            try
            {
                await connection.OpenAsync();
                switch (executeType)
                {
                    case ExecuteType.ExecuteReaderAsync:
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds;

                    case ExecuteType.ExecuteNonQueryAsync:
                        return await command.ExecuteNonQueryAsync();

                    case ExecuteType.ExecuteScalarAsync:
                        return await command.ExecuteScalarAsync();

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void ShowQueryResult(object result)
        {
            switch (result)
            {
                case DataSet dataSet:
                    foreach (DataTable table in dataSet.Tables)
                    {
                        foreach (DataColumn column in table.Columns)
                            Console.Write($"{column.ColumnName}\t");
                        Console.WriteLine();

                        foreach (DataRow row in table.Rows)
                        {
                            var cells = row.ItemArray;

                            foreach (var cell in cells)
                                Console.Write($"{cell}\t");

                            Console.WriteLine();
                        }
                    }
                    break;

                case decimal:
                    Console.WriteLine($"Результат: {result}");
                    break;

                case int:
                    Console.WriteLine($"Добавлено обьектов: {result}");
                    break;

                case null:
                    Console.WriteLine("Нет результата.");
                    break;

                default:
                    break;
            }
        }
    }
}
