using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common.DAL
{
    internal class LocationDAL : DAL
    {

        private static readonly LocationDAL instance = new LocationDAL();

        public static LocationDAL Instance { get { return instance; } }

        static LocationDAL() { }
        private LocationDAL() { }

        public Location? CreateLocation(string name, string address, string postalcode, string city, string country, string description)
        {
            Location? location = null;
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();
                    string insertLocationStatement = @"INSERT INTO LOCATION (name, address, postalcode, city, country, description) VALUES (@name, @address, @postalcode, @city, @country, @description); SELECT CAST(@@IDENTITY AS INT);";
                    string getLocationStatement = @"SELECT name, address, postalcode, city, country, description FROM LOCATION WHERE locationid = @locationid;";
                    int locationid;

                    using (SqlTransaction sqlTransaction = sqlconnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(insertLocationStatement, sqlconnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@postalcode", postalcode);
                            cmd.Parameters.AddWithValue("@city", city);
                            cmd.Parameters.AddWithValue("@country", country);
                            cmd.Parameters.AddWithValue("@description", description);

                            locationid = (int) cmd.ExecuteScalar();
                        }

                        using (SqlCommand cmd = new SqlCommand(getLocationStatement, sqlconnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@locationid", locationid);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }

                                while (reader.Read())
                                {
                                    string _name = (string)reader["name"];
                                    string _address = (string)reader["address"];
                                    string _postalcode = (string)reader["postalcode"];
                                    string _city = (string)reader["city"];
                                    string _country = (string)reader["country"];
                                    string _description = (string)reader["description"];

                                    location = new Location(_name, _address, _postalcode, _city, _country, _description);
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

            return location;
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
            catch (Exception)
            {
                return false;
            }
        }
        public List<Location>? GetAllLocations()
        {
            List<Location> locations = new List<Location>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string query = "SELECT locationid, name, address, postalcode, city, country, description FROM LOCATION";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
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
                                    int _id = (int)sqlDataReader["locationid"];
                                    string _name = (string)sqlDataReader["name"];
                                    string _address = (string)sqlDataReader["address"];
                                    string _postalcode = (string)sqlDataReader["postalcode"];
                                    string _city = (string)sqlDataReader["city"];
                                    string _country = (string)sqlDataReader["country"];
                                    string _description = (string)sqlDataReader["description"];

                                    locations.Add(new Location(_id, _name, _address, _postalcode, _city, _country, _description));
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

            return locations;
        }
        public Location? GetLocation(int id)
        {
            Location? location = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT locationid, name, address, postalcode, city, country, description FROM LOCATION WHERE locationid = @locationid;";
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@locationid", id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }

                                reader.Read(); // Er hoeft maar 1x een locatie worden opgehaald.

                                int _id = (int)reader["locationid"];
                                string _name = (string)reader["name"];
                                string _address = (string)reader["address"];
                                string _postalcode = (string)reader["postalcode"];
                                string _city = (string)reader["city"];
                                string _country = (string)reader["country"];
                                string _description = (string)reader["description"];

                                location = new Location(_id, _name, _address, _postalcode, _city, _country, _description);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return location;
        }
    }
}
