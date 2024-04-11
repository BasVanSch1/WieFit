using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common.DAL
{
    internal class PlanningDAL : DAL
    {
        private static readonly PlanningDAL instance = new PlanningDAL();

        public static PlanningDAL Instance { get { return instance; } }

        static PlanningDAL() { }
        private PlanningDAL() { }

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
            }
            catch (Exception) // Catch all, nu tijdelijk geen error output. Als GUI wordt gemaakt zal er een pop-up komen met de error.
            {
                return false;
            }
            return true;
        }
    }
}
