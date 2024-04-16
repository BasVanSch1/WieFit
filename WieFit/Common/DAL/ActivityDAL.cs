using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal class ActivityDAL : DAL
    {
        private static readonly ActivityDAL instance = new ActivityDAL();

        public static ActivityDAL Instance { get { return instance; } }

        static ActivityDAL() { }
        private ActivityDAL() { }

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
        public bool PlanActivity(PlannedActivity plannedactivity, Location location)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO PLANNEDACTIVITY(locationid, activityid, startdatetime, enddatetime, coachusername) VALUES(@locationid, @activityid, @starttime, @endtime, @coachusername)";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                        {
                            sqlCommand.Parameters.AddWithValue("@locationid", location.Id);
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
        public Activity? GetActivity(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT * FROM ACTIVITY WHERE activityid = @activityid;";
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
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
                }
            }
            catch (Exception)
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
        public List<PlannedActivity>? GetPlannedActivities()
        {
            List<PlannedActivity> plannedActivity = null;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string plannedActivityStatement = "select PA.activityid, A.name AS ActivityName, A.description, PA.startdatetime, PA.enddatetime, L.name AS LocationName, U.name As CoachName FROM PLANNEDACTIVITY PA INNER JOIN ACTIVITY A ON PA.activityid = A.activityid INNER JOIN LOCATION L ON PA.locationid = L.locationid INNER JOIN USERS U ON PA.coachusername = U.username;";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(plannedActivityStatement, sqlConnection, sqlTransaction))
                        {
                            using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                            {
                                if (!sqlReader.HasRows)
                                {
                                    return null;
                                }

                                while (sqlReader.Read())
                                {
                                    int _activityId = (int)sqlReader["activityid"];
                                    string _activityName = (string)sqlReader["ActivityName"];
                                    string _activityDescription = (string)sqlReader["description"];
                                    DateTime _startdatetime = (DateTime)sqlReader["startdatetime"];
                                    DateTime _enddatetime = (DateTime)sqlReader["enddatetime"];
                                    string _locationName = (string)sqlReader["LocationName"];
                                    string _coachName = (string)sqlReader["CoachName"];

                                    plannedActivity.Add(new PlannedActivity(_activityId, _activityName, _activityDescription, _startdatetime, _enddatetime, Coach.GetCoach(_coachName)));
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

            return plannedActivity;
        }
        private Activity? MapActivity(SqlDataReader reader)
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
            catch (Exception)
            {
                return null;
            }
        }

    }
}
