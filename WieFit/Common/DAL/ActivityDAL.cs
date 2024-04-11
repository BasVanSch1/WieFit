﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    {

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

    }
}