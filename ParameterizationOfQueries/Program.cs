using Microsoft.Data.SqlClient;
using System.Data;

Console.WriteLine("***** Parameterization of queries *****");

string connectionString = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";

string sqlExpression = "INSERT INTO Users (Name, Age) VALUES (@name, @age)";
string name1 = "Tom', 35); INSERT INTO Users (Name, Age) VALUES ('Henry";
int age1 = 35;
string name2 = "Bob";
int age2 = 40;

using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();

    SqlCommand cmd = new SqlCommand(sqlExpression, connection);

    // ! Пользователь #1

    SqlParameter nameParameter = new SqlParameter("@name", name1);
    SqlParameter ageParameter = new SqlParameter("@age", age1);

    cmd.Parameters.Add(nameParameter);
    cmd.Parameters.Add(ageParameter);

    int number = await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Добавлено {number} обьектов");

    // ! Пользователь #2

    cmd = new SqlCommand(sqlExpression, connection);

    nameParameter = new SqlParameter("@name", SqlDbType.NVarChar, 100);
    nameParameter.Value = name2;
    ageParameter = new SqlParameter("@age", age2);

    cmd.Parameters.Add(nameParameter);
    cmd.Parameters.Add(ageParameter);

    number = await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Добавлено {number} обьектов");

    // ! Пользователь #3

    cmd.Parameters["@name"].Value = "Heralt";
    cmd.Parameters["@age"].Value = 100;

    number = await cmd.ExecuteNonQueryAsync();
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