using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal class ResultDAL : DAL
    {
        private static readonly ResultDAL instance = new ResultDAL();
        public static ResultDAL Instance { get { return instance; } }

        static ResultDAL() { }
        private ResultDAL() { }

        public bool AddResult(Result result, User user, Activity activity)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                {
                    string resultstatement = @"INSERT INTO RESULT(username, activityid, datetime, description, result) VALUES (@username, @activityid, @datetime, @description, @result)";
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(resultstatement, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@username", user.Username);
                            command.Parameters.AddWithValue("@activityid", activity.Id);
                            command.Parameters.AddWithValue("@datetime", result.Date);
                            command.Parameters.AddWithValue("@description", result.Description);
                            command.Parameters.AddWithValue("@result", result.Value);
                            command.ExecuteNonQuery();

                            transaction.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
            return true;
        }
    }
}
