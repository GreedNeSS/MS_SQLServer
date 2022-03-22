using Microsoft.Data.SqlClient;

Console.WriteLine("***** Transactions *****");

string connectStr = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";

using (SqlConnection connection = new SqlConnection(connectStr))
{
    try
    {
        await connection.OpenAsync();
        SqlTransaction transaction = connection.BeginTransaction();
        SqlCommand command = connection.CreateCommand();
        command.Transaction = transaction;

        command.CommandText = "INSERT INTO Users(Name, Age) VALUES ('Ruslan', 55)";
        await command.ExecuteNonQueryAsync();
        command.CommandText = "INSERT INTO Users(Name, Age) VALUES ('Руслан', 55)";
        await command.ExecuteNonQueryAsync();

        await transaction.CommitAsync();
        Console.WriteLine("Данные добавлены в базу данных!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.ReadLine();
}