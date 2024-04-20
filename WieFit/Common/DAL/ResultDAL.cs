using Microsoft.Data.SqlClient;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal class ResultDAL : DAL
    {
        private static readonly ResultDAL instance = new ResultDAL();
        public static ResultDAL Instance { get { return instance; } }

        static ResultDAL() { }
        private ResultDAL() { }

        public bool AddResult(Result result, User user)
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
                            command.Parameters.AddWithValue("@activityid", result.Activity.Id);
                            command.Parameters.AddWithValue("@datetime", result.Date);
                            command.Parameters.AddWithValue("@description", result.Description);
                            command.Parameters.AddWithValue("@result", result.Value);
                            command.ExecuteNonQuery();

                            transaction.Commit();
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
        public List<Result>? GetResultsFromStudent(Student student)
        {
            List<Result> results = new List<Result>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string resultStatement = @"select R.activityid, A.name AS ActivityName, A.description AS ActivityDescription, R.datetime, R.description, r.result FROM RESULT R INNER JOIN ACTIVITY A ON R.activityid = A.activityid WHERE R.username = @username ORDER BY R.datetime;";
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(resultStatement, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@username", student.Username);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }

                                while (reader.Read())
                                {
                                    int _activityId = (int)reader["activityid"];
                                    string _activityName = (string)reader["ActivityName"];
                                    string _activityDescription = (string)reader["ActivityDescription"];
                                    DateTime _datetime = (DateTime)reader["datetime"];
                                    string _description = (string)reader["description"];
                                    decimal _result = (decimal)reader["result"];

                                    results.Add(new Result(_datetime, _description, _result, new Activity(_activityId, _activityName, _activityDescription)));
                                }
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return results;
        }
    }
}
