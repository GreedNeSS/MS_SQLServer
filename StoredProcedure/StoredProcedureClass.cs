using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace StoredProcedure
{
    internal class StoredProcedureClass
    {
        public enum ExecuteType
        {
            ExecuteReaderAsync,
            ExecuteNonQueryAsync,
            ExecuteScalarAsync,
        }

        public async Task CreateProcedureAsync(string connectionString, string sqlExpression)
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

        public async Task ExecutingProcedureAsync(string connectionString,
            string procedureName, ExecuteType executeType, Dictionary<string, string> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    int count = reader.FieldCount;

                                    for (int i = 0; i < count; i++)
                                        Console.Write(reader.GetName(i) + "\t");

                                    Console.WriteLine();

                                    while (await reader.ReadAsync())
                                    {
                                        for (int i = 0; i < count; i++)
                                            Console.Write(reader.GetValue(i) + "\t");

                                        Console.WriteLine();
                                    }
                                }
                            }
                            break;

                        case ExecuteType.ExecuteNonQueryAsync:
                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine("Запрос выполнен!");
                            break;

                        case ExecuteType.ExecuteScalarAsync:
                            object num = await command.ExecuteScalarAsync();
                            Console.WriteLine($"Result: {num}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
