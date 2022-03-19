using Microsoft.Data.SqlClient;

Console.WriteLine("***** Connection Pool *****");

string connectionString1 = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
string connectionString2 = "Server=(localdb)\\mssqllocaldb;Database=tempdb;Trusted_Connection=True;";

using (SqlConnection sqlConnection = new(connectionString1))
{
    await sqlConnection.OpenAsync();
    Console.WriteLine($"Id: {sqlConnection.ClientConnectionId}");
}

using (SqlConnection sqlConnection = new(connectionString2))
{
    await sqlConnection.OpenAsync();
    Console.WriteLine($"Id: {sqlConnection.ClientConnectionId}");
}

//SqlConnection.ClearAllPools();

using (SqlConnection sqlConnection = new(connectionString1))
{
    await sqlConnection.OpenAsync();
    Console.WriteLine($"Id: {sqlConnection.ClientConnectionId}");
}

Console.WriteLine("Программа завершила работу!");
Console.Read();