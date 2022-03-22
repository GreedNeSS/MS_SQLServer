using Microsoft.Data.SqlClient;
using System.Data;

Console.WriteLine("***** Output parameters of stored procedures *****");
Console.WriteLine("Введите имя человека: ");
string name = Console.ReadLine();

string connectStr = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";
string proc = @"CREATE PROCEDURE [dbo].[sp_GetAgeRange]
    @name nvarchar(50),
    @minAge int out,
    @maxAge int out
AS
    SELECT @minAge = MIN(Age), @maxAge = MAX(Age) 
    FROM Users 
    WHERE Name LIKE '%' + @name + '%'";
string procedureName = "sp_GetAgeRange";

int minAge, maxAge;

using (SqlConnection connection = new SqlConnection(connectStr))
{
    SqlCommand sqlCommand = new SqlCommand(proc, connection);
    try
    {
        await connection.OpenAsync();
        await sqlCommand.ExecuteNonQueryAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }


    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.CommandText = procedureName;

    SqlParameter nameParam = new SqlParameter("@name", name);
    SqlParameter minAgeParam = new SqlParameter
    {
        ParameterName = "@minAge",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Output,
    };
    SqlParameter maxAgeParam = new SqlParameter
    {
        ParameterName = "@maxAge",
        DbType = DbType.Int32,
        Direction = ParameterDirection.Output,
    };

    sqlCommand.Parameters.Add(nameParam);
    sqlCommand.Parameters.Add(minAgeParam);
    sqlCommand.Parameters.Add(maxAgeParam);

    try
    {
        await sqlCommand.ExecuteNonQueryAsync();

        object param1 = sqlCommand.Parameters["@minAge"].Value;
        object param2 = sqlCommand.Parameters["@maxAge"].Value;

        Console.WriteLine($"Minimal age: {param1}");
        Console.WriteLine($"Maximal age: {param2}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
