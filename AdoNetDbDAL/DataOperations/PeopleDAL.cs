using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoNetDbDAL.Models;

namespace AdoNetDbDAL.DataOperations
{
    public class PeopleDAL
    {
        private readonly string _connectionString;
        private SqlConnection _sqlConnection = null;

        public PeopleDAL(): this(
            @"Data Source = localhost;Integrated Security=true;Initial Catalog=adonetdb")
        {

        }

        public PeopleDAL(string connectionString) 
            => _connectionString = connectionString;

        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.Open();
        }

        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }

        public List<Person> GetAllPeople()
        {
            OpenConnection();
            List<Person> people = new List<Person>();
            string sql = "Select * From Users";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    people.Add(new Person
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Age = reader.GetInt32("Age")
                    });
                }
                reader.Close();
            }
            return people;
        }

        public Person? GetPerson(int id)
        {
            OpenConnection();
            Person person = null;
            string sql = $"Select * From Users Where Id = {id}";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    person = new Person
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Age = reader.GetInt32("Age")
                    };
                }
                reader.Close();
            }
            return person;
        }

        public Person? GetPerson(string name)
        {
            OpenConnection();
            Person person = null;
            string sql = $"Select * From Users Where Name = @Name";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@Name";
                parameter.Value = name;
                command.Parameters.Add(parameter);
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    person = new Person
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Age = reader.GetInt32("Age")
                    };
                }
                reader.Close();
            }
            return person;
        }

        public void InsertAuto(string name, int age)
        {
            OpenConnection();
            string sql = "Insert Into Users (Name, Age) Values (@Name, @Age)";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = name
                };
                SqlParameter ageParam = new SqlParameter
                {
                    ParameterName = "@Age",
                    Value = age
                };

                command.Parameters.Add(nameParam);
                command.Parameters.Add(ageParam);
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public void InsertAuto(Person person)
        {
            OpenConnection();
            string sql = "Insert Into Users" +
                         "(Name, Age) Values" +
                         "(@Name, @Age)";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = person.Name,
                    SqlDbType = SqlDbType.Char,
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter
                {
                    ParameterName = "@Age",
                    Value = person.Age,
                    SqlDbType = SqlDbType.Int
                };

                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void DeletePerson(int id)
        {
            OpenConnection();
            string sql = $"delete from Users where Id = {id}";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            CloseConnection();
        }

        public void UpdatePersonName(int id, string newName)
        {
            OpenConnection();
            string sql = "Update Users Set Name = @Name Where Id = @Id";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = newName
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = id
                };
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public void SimpleTransactionExample(bool throwEx, int personId)
        {
            OpenConnection();
            string name;
            int age;
            SqlCommand command = new SqlCommand($"Select * From Users Where Id = @id",
                _sqlConnection);
            command.Parameters.AddWithValue("@id", personId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    name = reader.GetString(2);
                    age = reader.GetInt32(1);

                    Console.WriteLine($"Name: {name}, Age: {age}");
                }
                else
                {
                    Console.WriteLine("Пользователь не найден!");

                    CloseConnection();
                    return;
                }
            }

            SqlCommand cmdRemove = new SqlCommand($"Delete From Users Where Id = @id",
                _sqlConnection);
            cmdRemove.Parameters.AddWithValue("@id", personId);

            SqlCommand cmdInsert = new SqlCommand($"Insert Into Users " +
                $"(Name, Age) Values(@name, @age)", _sqlConnection);
            cmdInsert.Parameters.AddWithValue("@name", name);
            cmdInsert.Parameters.AddWithValue("@age", age);

            SqlTransaction transaction = null;
            try
            {
                transaction = _sqlConnection.BeginTransaction();

                cmdInsert.Transaction = transaction;
                cmdRemove.Transaction = transaction;

                cmdInsert.ExecuteNonQuery();
                cmdRemove.ExecuteNonQuery();

                if (throwEx)
                {
                    throw new Exception("DB Error! transaction rollback!");
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                transaction?.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
