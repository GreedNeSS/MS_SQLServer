using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadUploadFiles
{
    internal static class DatabaseQueryCreator
    {

        public static async Task SqlQueryAsync(string connectStr, string sqlExpression)
        {
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sqlExpression;
                try
                {
                    await connection.OpenAsync();
                    await sqlCommand.ExecuteNonQueryAsync();

                    Console.WriteLine("Запрос выполнен!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static async Task SaveFileToDatabaseAsync(string connectStr,
            string tableName, string fileName, string title)
        {
            string shortName = fileName.Substring(fileName.LastIndexOf('\\') + 1);

            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO {tableName} VALUES (@FilaName, @Title, @ImageData)",
                    connection);

                command.Parameters.AddWithValue("@FilaName", shortName);
                command.Parameters.AddWithValue("@Title", title);

                byte[] buffer;

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    command.Parameters.Add("@ImageData", System.Data.SqlDbType.Image,
                        buffer.Length);
                }

                command.Parameters["@ImageData"].Value = buffer;

                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Файл сохранён!");
            }
        }

        public static async Task<List<Image>> LoadFileFromDatabaseAsync(string connectStr,
             string sqlExpression)
        {
            List<Image> images = new List<Image>();

            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int id = (int)reader["Id"];
                        string fileNAme = (string)reader["FileName"];
                        string title = (string)reader["Title"];
                        byte[] data = (byte[])reader["ImageData"];

                        images.Add(new Image(id, fileNAme, title, data));
                    }
                }

                return images;
            }
        }
    }
}
