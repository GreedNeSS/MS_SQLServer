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

StoredProcedureClass procedureClass = new StoredProcedureClass();

await procedureClass.CreateProcedureAsync(conStr, proc1);
await procedureClass.CreateProcedureAsync(conStr, proc2);

Console.WriteLine();

await procedureClass.ExecutingProcedureAsync(conStr, "sp_InsertUser",
    StoredProcedureClass.ExecuteType.ExecuteScalarAsync, parameters);

await procedureClass.ExecutingProcedureAsync(conStr, "sp_GetUsers", StoredProcedureClass.ExecuteType.ExecuteReaderAsync);