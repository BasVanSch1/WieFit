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

        public bool CreateUser(User user, string password, char type)
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

        public bool Addlocation(Location location)
        {
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();
                    string query = "INSERT INTO LOCATION (name, adress, postalcode, country) VALUES (@name, @adress, @postalcode, @country);";
                    using (SqlTransaction sqlTransaction = sqlconnection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, sqlconnection, sqlTransaction))
                        {
                            command.Parameters.AddWithValue("@name", location.name);
                            command.Parameters.AddWithValue("@adress", location.adress);
                            command.Parameters.AddWithValue("@postalcode", location.postalcode);
                            command.Parameters.AddWithValue("@country", location.country);

                            command.ExecuteNonQuery();

                            sqlTransaction.Commit();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        public bool DeleteLocation(Location location)
        {
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();
                    string query = "DELETE FROM LOCATION WHERE locationid = locationid";
                    using (SqlTransaction sqlTransaction = sqlconnection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, sqlconnection, sqlTransaction))
                        {
                            {
                                command.Parameters.AddWithValue("@Location_id", location.id);
                                command.ExecuteNonQuery();
                            }

                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}
