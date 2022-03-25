using AdoNetDbDAL.DataOperations;
using AdoNetDbDAL.Models;
using AdoNetDbDAL.BulkImport;

Console.WriteLine("***** AdoNetDb Client *****");

PeopleDAL dal = new PeopleDAL();
List<Person> list = dal.GetAllPeople();

Console.WriteLine("*** People ***");

foreach (var item in list)
{
    Console.WriteLine($"{item}");
}

Person person = dal.GetPerson(15);
Console.WriteLine($"\ndal.GetPerson(15): {person}");
person = dal.GetPerson("Руслан");
Console.WriteLine($"dal.GetPerson(\"Руслан\"): {person}");

person = new Person
{
    Name = "Виктор",
    Age = 67
};

dal.InsertAuto("Геральт", 120);
dal.InsertAuto(person);
dal.DeletePerson(4);
dal.UpdatePersonName(5, "Greed");

Console.WriteLine("\n*** People ***");

list = dal.GetAllPeople();

foreach (var item in list)
{
    Console.WriteLine($"{item}");
}

Console.WriteLine("\n=> SimpleTransactionExample(true, 10): ");

dal.SimpleTransactionExample(true, 10);

Console.WriteLine("\n*** People ***");

list = dal.GetAllPeople();

foreach (var item in list)
{
    Console.WriteLine($"{item}");
}

Console.WriteLine("\n***** Do Bulk Copy *****");

List<Person> people = new List<Person>
{
    new Person{Name = "Rick", Age = 37},
    new Person{Name = "Bill", Age = 40},
    new Person{Name = "Rufus", Age = 12},
    new Person{Name = "Fill", Age = 23},
    new Person{Name = "Fred", Age = 56},
};

ProcessBulkImport.ExecuteBulkImport(people, "Users");

Console.WriteLine("\n*** People ***");

list = dal.GetAllPeople();

foreach (var item in list)
{
    Console.WriteLine($"{item}");
}