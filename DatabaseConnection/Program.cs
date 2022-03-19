using Microsoft.Data.SqlClient;
using System.Data;

string connectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True";


using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();
    Console.WriteLine("Подключение открыто!");
    Console.WriteLine("\nСвойства подключения:");
    Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
    Console.WriteLine($"\tБаза данных: {connection.Database}");
    Console.WriteLine($"\tСервер: {connection.DataSource}");
    Console.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
    Console.WriteLine($"\tСостояние: {connection.State}");
    Console.WriteLine($"\tWorkstation Id: {connection.WorkstationId}");
}

Console.WriteLine("Подключение закрыто");
Console.WriteLine("Программа завершила работу.");
Console.Read();
