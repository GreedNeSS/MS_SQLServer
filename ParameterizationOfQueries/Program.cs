using Microsoft.Data.SqlClient;

Console.WriteLine("***** Parameterization of queries *****");

string connectionString = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";

string name = "Tom', 35); INSERT INTO Users (Name, Age) VALUES ('Henry";
string sqlExpression = "INSERT INTO Users (Name, Age) VALUES (@name, @age)";
int age = 35;

using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();

    SqlCommand cmd = new SqlCommand(sqlExpression, connection);
    SqlParameter nameParameter = new SqlParameter("@name", name);
    SqlParameter ageParameter = new SqlParameter("@age", age);


    cmd.Parameters.Add(nameParameter);
    cmd.Parameters.Add(ageParameter);

    int number = await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Добавлено {number} обьектов");

    cmd.CommandText = "SELECT * FROM Users";
    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
    {
        if (reader.HasRows)
        {
            string col1 = reader.GetName(0);
            string col2 = reader.GetName(1);
            string col3 = reader.GetName(2);

            Console.WriteLine($"{col1}\t{col3}\t{col2}");

            while(await reader.ReadAsync())
            {
                int idReq = reader.GetInt32(0);
                int ageReq = reader.GetInt32(1);
                string nameReq = reader.GetString(2);

                Console.WriteLine($"{idReq}\t{nameReq}\t{ageReq}");
            }
        }
    }
}