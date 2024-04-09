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
            catch (Exception ex)
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
                    string query = "INSERT INTO LOCATION (planningid, name, adress, postalcode, city, country) VALUES (@planningid, @name, @adress, @postalcode, @city, @country);";
                    using (SqlTransaction sqlTransaction = sqlconnection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, sqlconnection, sqlTransaction))
                        {
                            command.Parameters.AddWithValue("@planningid", location.Planning.Id);
                            command.Parameters.AddWithValue("@name", location.Name);
                            command.Parameters.AddWithValue("@adress", location.Address);
                            command.Parameters.AddWithValue("@postalcode", location.Postalcode);
                            command.Parameters.AddWithValue("@city", location.City);
                            command.Parameters.AddWithValue("@country", location.Country);

                            command.ExecuteNonQuery();
                            
                            command.CommandText = "SELECT CAST(@@Identity as INT);";
                            var id = (int)command.ExecuteScalar();
                            location.Id = id;
                          
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
                    string query = "DELETE FROM LOCATION WHERE locationid = @locationid";
                    using (SqlTransaction sqlTransaction = sqlconnection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, sqlconnection, sqlTransaction))
                        {
                            {
                                command.Parameters.AddWithValue("@locationid", location.Id);
                                command.ExecuteNonQuery();
                              
                                sqlTransaction.Commit();
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
      
        public bool CreatePlanning(Planning planning)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string userStatement = @"INSERT INTO PLANNING (isactive)  VALUES (@isactive)";
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(userStatement, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@isactive", planning.IsActive);

                            command.ExecuteNonQuery();
                          
                            //Get ID from database
                            command.CommandText = "SELECT CAST(@@Identity as INT);";
                            var id = (int)command.ExecuteScalar();
                            planning.Id = id;

                            transaction.Commit();
                        }
                    }
                }
            } catch(Exception) // Catch all, nu tijdelijk geen error output. Als GUI wordt gemaakt zal er een pop-up komen met de error.
            {
                return false;
            }
            return true;
        }
    }
}
