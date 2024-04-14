﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal class UserDAL : DAL
    {
        private static readonly UserDAL instance = new UserDAL();

        public static UserDAL Instance { get { return instance; } }

        static UserDAL() { }
        private UserDAL() { }

        public bool CreateUser(User user, string password, char type)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string userStatement = @"INSERT INTO USERS (username, name, email, password, address, phonenumber, age, gender, type) VALUES ( @username, @name, @email, @password, @address, @phonenumber, @age, @gender, @type );";

                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(userStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@username", user.Username);
                            cmd.Parameters.AddWithValue("@name", user.Name);
                            cmd.Parameters.AddWithValue("@email", user.Email);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@address", user.Address);
                            cmd.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
                            cmd.Parameters.AddWithValue("@age", user.Age);
                            cmd.Parameters.AddWithValue("@gender", user.Gender);
                            cmd.Parameters.AddWithValue("@type", type);

                            cmd.ExecuteNonQuery();

                            sqlTransaction.Commit();
                        }
                    }
                }
            }
            catch (Exception) // Catch all, nu tijdelijk geen error output. Als GUI wordt gemaakt zal er een pop-up komen met de error.
            {
                return false;
            }

            return true;
        }
        public bool CheckPassword(string username, string password)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string userStatement = @"SELECT COUNT(username) FROM USERS WHERE username = @username and password = @password;";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(userStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);

                            int count = (int)cmd.ExecuteScalar();

                            if (count != 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public User? GetUser(string username, string password)
        {
            User user = null;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string userStatement = @"SELECT username, name, email, address, phonenumber, age, gender FROM USERS WHERE username = @username and password = @password;";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(userStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }

                                reader.Read(); // Er hoeft maar 1x een user worden opgehaald.

                                string _username = (string)reader["username"];
                                string _name = (string)reader["name"];
                                string _email = (string)reader["email"];
                                string _address = (string)reader["address"];
                                string _phonenumber = (string)reader["phonenumber"];
                                int _age = (int)reader["age"];
                                char _gender = reader["gender"].ToString().ToCharArray().First();

                                user = new User(_username, _name, _email, _address, _phonenumber, _age, _gender);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }
        public List<Coach>? GetAllCoach()
        {
            List<Coach>? coaches = new List<Coach>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT username,name, email, address, phonenumber, age, gender FROM USERS WHERE type = 'C';";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                        {
                            using (SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }
                                while (reader.Read())
                                {
                                    string _username = (string)reader["username"];
                                    string _name = (string)reader["name"];
                                    string _email = (string)reader["email"];
                                    string _address = (string)reader["address"];
                                    string _phonenumber = (string)reader["phonenumber"];
                                    int _age = (int)reader["age"];
                                    char _gender = reader["gender"].ToString().ToCharArray().First();

                                    coaches.Add(new Coach(_username, _name, _email, _address, _phonenumber, _age, _gender));
                                }
                            }
                        }
                        sqlTransaction.Commit();
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
                return null;
            }
            return coaches;
        }
        public Coach? GetCoach(string username)
        {
            Coach? coach = null;
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT username,name, email, address, phonenumber, age, gender FROM USERS WHERE type = 'C' AND username = @username; ";
                    connection.Open();

                    using(SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, connection,transaction))
                        {
                            command.Parameters.AddWithValue("@username", username);

                            using(SqlDataReader reader = command.ExecuteReader()) 
                            {
                                
                                if (reader.Read())
                                {
                                    string _username = (string)reader["username"];
                                    string _name = (string)reader["name"];
                                    string _email = (string)reader["email"];
                                    string _address = (string)reader["address"];
                                    string _phonenumber = (string)reader["phonenumber"];
                                    int _age = (int)reader["age"];
                                    char _gender = reader["gender"].ToString().ToCharArray().First();

                                    coach = new Coach(_username, _name, _email, _address, _phonenumber, _age, _gender);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
                return null;
            }
            return coach;
        }
        public List<Student>? GetAllStudents()
        {
            List<Student> students = new List<Student>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT username,name, email, address, phonenumber, age, gender FROM USERS WHERE type = 'S'";
                    connection.Open();

                    using(SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using(SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            using(SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }
                                while (reader.Read())
                                {
                                    string _username = (string)reader["username"];
                                    string _name = (string)reader["name"];
                                    string _email = (string)reader["email"];
                                    string _address = (string)reader["address"];
                                    string _phonenumber = (string)reader["phonenumber"];
                                    int _age = (int)reader["age"];
                                    char _gender = reader["gender"].ToString().ToCharArray().First();

                                    students.Add(new Student(_username, _name, _email, _address, _phonenumber, _age, _gender));
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
                return null;
            }
            return students;
        }
        public Student GetStudent(string username)
        {
            Student? student = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT username,name, email, address, phonenumber, age, gender FROM USERS WHERE type = 'S' AND username = @username; ";
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@username", username);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {

                                if (reader.Read())
                                {
                                    string _username = (string)reader["username"];
                                    string _name = (string)reader["name"];
                                    string _email = (string)reader["email"];
                                    string _address = (string)reader["address"];
                                    string _phonenumber = (string)reader["phonenumber"];
                                    int _age = (int)reader["age"];
                                    char _gender = reader["gender"].ToString().ToCharArray().First();

                                    student = new Student(_username, _name, _email, _address, _phonenumber, _age, _gender);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
            return student;
        }
        public bool GiveAdvise(Advice advice)
        {
            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO ADVICE(student, coach, advice) VALUES(@studentusername, @coachusername, @advice)";
                    connection.Open();

                    using(SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@studentusername", advice.student.Username);
                            command.Parameters.AddWithValue("@coachusername", advice.coach.Username);
                            command.Parameters.AddWithValue("@advice", advice.Description);

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
                return false;
            }
            return true;
        }
        public List<Advice>? GetAdvice(string username)
        {
            List<Advice>? advices = new List<Advice>();
            
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT A.advice, A.adviceid,A.coach, U.username,U.name, U.email, U.address, U.phonenumber, U.age, U.gender FROM ADVICE A JOIN USERS U ON U.username = A.coach WHERE student = @username";
                    connection.Open();

                    using(SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using(SqlCommand command = new SqlCommand(query,connection, transaction))
                        {
                            command.Parameters.AddWithValue("@username", username);

                            using(SqlDataReader reader = command.ExecuteReader()) 
                            {
                                while (reader.Read())
                                {
                                    int id = (int)(reader["adviceid"]);
                                    string Cusername = (string)reader["coach"];
                                    string Description = (string)reader["advice"];
                                    string _username = (string)reader["username"];
                                    string _name = (string)reader["name"];
                                    string _email = (string)reader["email"];
                                    string _address = (string)reader["address"];
                                    string _phonenumber = (string)reader["phonenumber"];
                                    int _age = (int)reader["age"];
                                    char _gender = reader["gender"].ToString().ToCharArray().First();

                                    Coach coach = new Coach(_username, _name, _email, _address, _phonenumber, _age, _gender);
                                    advices.Add(new Advice(id, Description, coach));
                                }
                            }
                        }
                    }
                }

            }catch (Exception ex)
            {
                throw ex;
                return null;
            }
            return advices;
        }
    }
}
