using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal class DAL
    {
        private static readonly DAL instance = new DAL();
        private readonly string connectionString = @"Data Source=.;Initial Catalog=WieFit;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";

        public static DAL Instance { get { return instance; } }

        static DAL() { }
        private DAL() { }

        public bool CreateUser(User user, string password)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string userStatement = @"INSERT INTO USERS (username, name, email, password, adress, phonenumber, age, gender, type) VALUES ( @username, @name, @email, @password, @adress, @phonenumber, @age, @gender, @type );";

                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(userStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@username", user.Username);
                            cmd.Parameters.AddWithValue("@name", user.Name);
                            cmd.Parameters.AddWithValue("@email", user.Email);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@adress", user.Adress);
                            cmd.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
                            cmd.Parameters.AddWithValue("@age", user.Age);
                            cmd.Parameters.AddWithValue("@gender", user.Gender);
                            cmd.Parameters.AddWithValue("@type", 'S'); // Altijd student, anders moet je voor elke type een nieuwe methode aanmaken

                            cmd.ExecuteNonQuery();

                            sqlTransaction.Commit();
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

        public bool CreateActivity(Common.Activity activity)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string query = @"INSERT INTO ACTIVITY(name, description) VALUES(@name, @description);";
                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                        {
                            sqlCommand.Parameters.AddWithValue("@name", activity.Name);
                            sqlCommand.Parameters.AddWithValue("@description", activity.Description);
                            sqlCommand.ExecuteNonQuery();
                            sqlTransaction.Commit();
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

        public bool CreateActivity(Common.Activity activity)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string query = @"INSERT INTO ACTIVITY(name, description) VALUES(@name, @description);";
                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                        {
                            sqlCommand.Parameters.AddWithValue("@name", activity.Name);
                            sqlCommand.Parameters.AddWithValue("@description", activity.Description);
                            sqlCommand.ExecuteNonQuery();
                            sqlTransaction.Commit();
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
    }
}
