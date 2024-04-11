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
  
        public bool CreateActivity(Activity activity)
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
        
      public bool PlanActivity(PlannedActivity plannedactivity, Planning planning)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO PLANNEDACTIVITY(planningid, activityid, startdatetime, enddatetime, coachusername) VALUES(@planningid, @activityid,@starttime, @endtime,@coachusername)";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                        {
                            sqlCommand.Parameters.AddWithValue("@planningid", planning.Id);
                            sqlCommand.Parameters.AddWithValue("@activityid", plannedactivity.Id);
                            sqlCommand.Parameters.AddWithValue("@starttime", plannedactivity.StartTime);
                            sqlCommand.Parameters.AddWithValue("@endtime", plannedactivity.EndTime);
                            sqlCommand.Parameters.AddWithValue("@coachusername", plannedactivity.Coach.Username);
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
      
      public Activity GetActivity(int id)
        {
            try
            {
                using ( SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT * FROM ACTIVITY WHERE activityid = @activityid;";
                    connection.Open();

                    using ( SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@activityid", id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                return MapActivity(reader);
                            }
                        }
                    }
                    {
                        
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

      private Activity MapActivity(SqlDataReader reader)
        {
            try
            {
                Activity activity = new Activity(
                    Convert.ToInt32(reader["activityid"]),
                    reader["name"].ToString(),
                    reader["description"].ToString()
                );
                return activity;
            }
            catch (Exception e)
            {
                return null;
            }
        }
      
      public List<Activity>? GetAllActivities()
        {
            List<Activity> activities = new List<Activity>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {

                    string query = "SELECT activityid, name, description FROM ACTIVITY";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction()) // wss niet eens nodig maarja..
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                        {
                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                if (!sqlDataReader.HasRows)
                                {
                                    return null;
                                }

                                while (sqlDataReader.Read())
                                {
                                    int _id = (int)sqlDataReader["activityid"];
                                    string _name = (string)sqlDataReader["name"];
                                    string _description = (string)sqlDataReader["description"];

                                    activities.Add(new Activity(_id, _name, _description));
                                }
                            }
                        }
                        sqlTransaction.Commit();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return activities;
        }
      
    }
}
