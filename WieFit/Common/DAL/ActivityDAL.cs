using Microsoft.Data.SqlClient;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal class ActivityDAL : DAL
    {
        private static readonly ActivityDAL instance = new ActivityDAL();

        public static ActivityDAL Instance { get { return instance; } }

        static ActivityDAL() { }
        private ActivityDAL() { }

        public Activity? CreateActivity(string name, string description)
        {
            Activity? activity = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string insertActivity = @"INSERT INTO ACTIVITY(name, description) VALUES(@name, @description); SELECT CAST(@@IDENTITY AS INT);";
                    string getActivity = @"SELECT name, description FROM ACTIVITY WHERE activityid = @activityid";

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        int activityid;

                        using (SqlCommand cmd = new SqlCommand(insertActivity, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@description", description);
                            activityid = (int)cmd.ExecuteScalar();
                        }

                        using (SqlCommand cmd = new SqlCommand(getActivity, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@activityid", activityid);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }

                                while (reader.Read())
                                {
                                    string _name = (string)reader["name"];
                                    string _description = (string)reader["description"];

                                    activity = new Activity(activityid, _name, _description);
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

            return activity;
        }
        public PlannedActivity? PlanActivity(Activity activityTemplate, DateTime starttime, DateTime endtime, Coach coach, int locationId)
        {
            PlannedActivity? activity = null;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string insertActivityStatement = @"INSERT INTO PLANNEDACTIVITY(locationid, activityid, startdatetime, enddatetime, coachusername) VALUES(@locationid, @activityid, @starttime, @endtime, @coachusername);";
                    string getActivityStatement = @"select PA.activityid, A.name AS ActivityName, A.description, PA.startdatetime, PA.enddatetime, L.locationid, U.username As CoachUsername FROM PLANNEDACTIVITY PA INNER JOIN ACTIVITY A ON PA.activityid = A.activityid INNER JOIN LOCATION L ON PA.locationid = L.locationid INNER JOIN USERS U ON PA.coachusername = U.username WHERE PA.activityid = @activityid and PA.locationid = @locationid and PA.startdatetime = @starttime;";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(insertActivityStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@locationid", locationId);
                            cmd.Parameters.AddWithValue("@activityid", activityTemplate.Id);
                            cmd.Parameters.AddWithValue("@starttime", starttime);
                            cmd.Parameters.AddWithValue("@endtime", endtime);
                            cmd.Parameters.AddWithValue("@coachusername", coach.Username);

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(getActivityStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@locationid", locationId);
                            cmd.Parameters.AddWithValue("@activityid", activityTemplate.Id);
                            cmd.Parameters.AddWithValue("@starttime", starttime);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }

                                while (reader.Read())
                                {
                                    int _activityId = (int)reader["activityid"];
                                    string _activityName = (string)reader["ActivityName"];
                                    string _activityDescription = (string)reader["description"];
                                    DateTime _startdatetime = (DateTime)reader["startdatetime"];
                                    DateTime _enddatetime = (DateTime)reader["enddatetime"];
                                    int _locationId = (int)reader["locationid"];
                                    string _coachUsername = (string)reader["CoachUsername"];

                                    activity = new PlannedActivity(_activityId, _activityName, _activityDescription, _startdatetime, _enddatetime, Coach.GetCoach(_coachUsername));
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

            return activity;
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
            List<PlannedActivity> plannedActivity = new List<PlannedActivity>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string plannedActivityStatement = "select PA.activityid, A.name AS ActivityName, A.description, PA.startdatetime, PA.enddatetime, L.locationid, U.username As CoachUsername FROM PLANNEDACTIVITY PA INNER JOIN ACTIVITY A ON PA.activityid = A.activityid INNER JOIN LOCATION L ON PA.locationid = L.locationid INNER JOIN USERS U ON PA.coachusername = U.username;";
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
                                    int _locationId = (int)sqlReader["locationid"];
                                    string _coachUsername = (string)sqlReader["CoachUsername"];

                                    plannedActivity.Add(new PlannedActivity(_activityId, _activityName, _activityDescription, _startdatetime, _enddatetime, Coach.GetCoach(_coachUsername)));
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
        public List<PlannedActivity>? GetPlannedActivities(Location location)
        {
            List<PlannedActivity> plannedActivity = new List<PlannedActivity>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    string plannedActivityStatement = "select PA.activityid, A.name AS ActivityName, A.description, PA.startdatetime, PA.enddatetime, L.locationid, U.username As CoachUsername FROM PLANNEDACTIVITY PA INNER JOIN ACTIVITY A ON PA.activityid = A.activityid INNER JOIN LOCATION L ON PA.locationid = L.locationid INNER JOIN USERS U ON PA.coachusername = U.username WHERE PA.locationid = @locationid ORDER BY PA.startdatetime;";
                    sqlConnection.Open();

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(plannedActivityStatement, sqlConnection, sqlTransaction))
                        {
                            cmd.Parameters.AddWithValue("@locationid", location.Id);

                            using (SqlDataReader sqlReader = cmd.ExecuteReader())
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
                                    int _locationId = (int)sqlReader["locationid"];
                                    string _coachUsername = (string)sqlReader["CoachUsername"];

                                    plannedActivity.Add(new PlannedActivity(_activityId, _activityName, _activityDescription, _startdatetime, _enddatetime, Coach.GetCoach(_coachUsername)));
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
