using Microsoft.Data.SqlClient;

Console.WriteLine("***** Query Output Parameters *****");

string connectionString = "Server=localhost;Database=adonetdb;Trusted_connection=True;Encrypt=False";
string sqlExpression = "INSERT INTO Users (Name, Age) VALUES (@name, @age); SET @id=SCOPE_IDENTITY()";
string newName = "Greed";
int newAge = 30;

using (SqlConnection connection = new SqlConnection(connectionString))
{
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    SqlParameter nameParam = new SqlParameter("@name", newName);
    SqlParameter ageParam = new SqlParameter("@age", newAge);
    SqlParameter idParam = new SqlParameter
    {
        DbType = System.Data.DbType.Int32,
        Direction = System.Data.ParameterDirection.Output,
        ParameterName = "@id",
    };

    command.Parameters.Add(nameParam);
    command.Parameters.Add(ageParam);
    command.Parameters.Add(idParam);

    try
    {
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Идентификатор нового пользователя: {idParam.Value}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

Console.Read();