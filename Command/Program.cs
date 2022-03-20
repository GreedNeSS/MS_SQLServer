using Microsoft.Data.SqlClient;

Console.WriteLine("***** Command *****");

string connectionMaster = "Server=localhost;Database=master;Trusted_Connection=True;Encrypt=False";
using (SqlConnection connection = new SqlConnection(connectionMaster))
{
    await connection.OpenAsync();

    SqlCommand command = new SqlCommand();
    command.CommandText = "CREATE DATABASE adonetdb";
    command.Connection = connection;

    await command.ExecuteNonQueryAsync();

    Console.WriteLine("База данный создана");
}

string connectionAdonetdb = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";
using (SqlConnection connection = new SqlConnection(connectionAdonetdb))
{
    await connection.OpenAsync();

    SqlCommand command = new SqlCommand();
    command.CommandText = "CREATE TABLE Users (Id INT PRIMARY KEY IDENTITY, Age INT NOT NULL, Name NVARCHAR(100) NOT NULL)";
    command.Connection = connection;
    await command.ExecuteNonQueryAsync();

    Console.WriteLine("Таблица Users создана");

    command.CommandText = "INSERT INTO Users (Age, Name) VALUES (30, 'Ruslan'), (23, 'Henry'), (45, 'Marcus')";
    int number = await command.ExecuteNonQueryAsync();

    Console.WriteLine($"Добавленно обьектов: {number}");

    command.CommandText = "UPDATE Users SET Age=20 WHERE Name='Henry'";
    number = await command.ExecuteNonQueryAsync();

    Console.WriteLine($"Обновлено обьектов: {number}");

    command.CommandText = "DELETE FROM Users WHERE Name='Marcus'";
    number = await command.ExecuteNonQueryAsync();

    Console.WriteLine($"Удалено обьектов: {number}");
}

Console.Read();