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

        public bool Addlocation(Location location)
        {
            try
            {
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();
                    string query = "INSERT INTO LOCATION (name, address, postalcode, city, country) VALUES (@name, @address, @postalcode, @city, @country);";
                    using (SqlTransaction sqlTransaction = sqlconnection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(query, sqlconnection, sqlTransaction))
                        {
                            command.Parameters.AddWithValue("@name", location.Name);
                            command.Parameters.AddWithValue("@address", location.Address);
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
        public List<Location>? GetAllLocations()
        {
            List<Location> locations = new List<Location>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string query = "SELECT locationid, name, address, postalcode, city, country  FROM LOCATION";
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

                                    locations.Add(new Location(_id, _name, _address, _postalcode, _city, _country));
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
                    string query = @"SELECT locationid, name, address, postalcode, city, country FROM LOCATION WHERE locationid = @locationid;";
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

                                location = new Location(_id, _name, _address, _postalcode, _city, _country);

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
