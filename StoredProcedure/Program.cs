using StoredProcedure;

Console.WriteLine("***** Stored procedure *****");

string conStr = "Server=localhost;Database=adonetdb;Trusted_Connection=True;Encrypt=False";

Console.WriteLine("Введите имя пользователя: ");
string name = Console.ReadLine();
Console.WriteLine("Введите возраст пользователя: ");
int age = int.Parse(Console.ReadLine());

Dictionary<string, string> parameters = new Dictionary<string, string>();
parameters["@name"] = name;
parameters["@age"] = age.ToString();

string proc1 = @"CREATE PROCEDURE [dbo].[sp_InsertUser] 
@name nvarchar(50), @age int 
AS 
INSERT INTO Users (Name, Age) VALUES (@name, @age)
SELECT SCOPE_IDENTITY()
GO";
string proc2 = @"CREATE PROCEDURE [dbo].[sp_GetUsers]
AS
SELECT * FROM Users
GO";

await StoredProcedureClass.CreateProcedureAsync(conStr, proc1);
await StoredProcedureClass.CreateProcedureAsync(conStr, proc2);

Console.WriteLine();

object result1 = await StoredProcedureClass.ExecutingProcedureAsync(conStr, "sp_InsertUser",
    ExecuteType.ExecuteScalarAsync, parameters);
object result2 = await StoredProcedureClass.ExecutingProcedureAsync(conStr, "sp_GetUsers",
    ExecuteType.ExecuteReaderAsync);

StoredProcedureClass.ShowQueryResult(result1);
StoredProcedureClass.ShowQueryResult(result2);