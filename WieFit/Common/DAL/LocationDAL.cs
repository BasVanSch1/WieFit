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
    }
}
