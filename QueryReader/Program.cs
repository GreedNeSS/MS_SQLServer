using Microsoft.Data.SqlClient;

Console.WriteLine("***** SQL Data Reader *****");

string connectionString = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";
string queryString = "SELECT * FROM Users";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();

    SqlCommand cmd = new SqlCommand(queryString, connection);
    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
    {
        if (reader.HasRows)
        {
            string column1 = reader.GetName(0);
            string column2 = reader.GetName(1);
            string column3 = reader.GetName(2);

            Console.WriteLine($"{column1}\t{column3}\t{column2}");

            while (await reader.ReadAsync())
            {
                //object id = reader.GetValue(0);
                //object age = reader.GetValue(1);
                //object name = reader.GetValue(2);

                //object id = reader[0];
                //object age = reader[1];
                //object name = reader[2];

                //object id = reader["Id"];
                //object age = reader["Age"];
                //object name = reader["Name"];

                object id = reader[column1];
                object age = reader[column2];
                object name = reader[column3];

                Console.WriteLine($"{id}\t{name}\t{age}");
            }
        }
    }

    Console.ReadLine();
}
