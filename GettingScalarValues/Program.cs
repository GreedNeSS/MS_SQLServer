using Microsoft.Data.SqlClient;

Console.WriteLine("***** Getting Scalar Values *****");

string connectionString = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();

    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
    object userCount = await command.ExecuteScalarAsync();

    command.CommandText = "SELECT MIN(Age) FROM Users";
    object minAge = await command.ExecuteScalarAsync();

    command.CommandText = "SELECT AVG(Age) FROM Users";
    object avgAge = await command.ExecuteScalarAsync();

    Console.WriteLine($"В таблице {userCount} обьектов!");
    Console.WriteLine($"Минимальный возраст {minAge}");
    Console.WriteLine($"Средний возраст {avgAge}");
}

Console.ReadLine();
