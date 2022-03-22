using LoadUploadFiles;

Console.WriteLine("***** Saving and Retrieving Files from the Database *****");

string connectStr1 = "Server=localhost;Database=master;Trusted_Connection=True;Encrypt=False";
string sqlExpression1 = "CREATE DATABASE filesdb";
string connectStr2 = "Server=localhost;Database=filesdb;Trusted_Connection=True;Encrypt=False";
string sqlExpression2 = @"CREATE TABLE Files
                            (Id INT PRIMARY KEY IDENTITY,
                             Title NVARCHAR(50) NOT NULL,
                             FileName NVARCHAR(50) NOT NULL,
                             ImageData varbinary(MAX))";
string sqlExpression3 = "SELECT * FROM Files";
string newPath = "F:\\NewImage.jpg";

await DatabaseQueryCreator.SqlQueryAsync(connectStr1, sqlExpression1);
await DatabaseQueryCreator.SqlQueryAsync(connectStr2, sqlExpression2);
await DatabaseQueryCreator.SaveFileToDatabaseAsync(connectStr2, "Files", @"F:\Image.jpg", "Скрин");
List<Image> images = await DatabaseQueryCreator.LoadFileFromDatabaseAsync(connectStr2, sqlExpression3);

if (images.Count > 0)
{
    using (FileStream fs = new FileStream(newPath, FileMode.OpenOrCreate))
    {
        await fs.WriteAsync(images[0].Data, 0, images[0].Data.Length);

        Console.WriteLine($"Изображение {images[0].Title} сохранено в файл {newPath}. Размер {images[0].Data.Length} байт.");
    }
}