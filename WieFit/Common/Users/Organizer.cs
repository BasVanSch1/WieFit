using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using WieFit.Common.DAL;

namespace WieFit.Common.Users
{
    internal class Organizer : User
    {
        private readonly ActivityDAL activityDAL = ActivityDAL.Instance;
        private static readonly UserDAL userDAL = UserDAL.Instance;
        public Organizer(string _username, string _name, string _email, string _address, string _phoneNumber, int _age, char _gender) :
            base (_username, _name, _email, _address, _phoneNumber, _age, _gender)
        {}

        public bool CreateUser(string _password)
        {
            return userDAL.CreateUser(this, _password, 'O');
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }

        public bool CreateActivity(Activity activity)
        {
            return activityDAL.CreateActivity(activity);
        }
        public bool PlanActivity(PlannedActivity plannedactivity, Location location)
        {
            return activityDAL.PlanActivity(plannedactivity, location);
        }
        public List<Activity>? GetAllActivities()
        {
            return activityDAL.GetAllActivities();
        }
        public Location GetLocation(int id)
        {
            return locationDAL.GetLocation(id);
        }
        public List<Coach> GetAllCoaches()
        {
            return userDAL.GetAllCoach();
        }
        public static Coach GetCoach(string username)
        {
            return userDAL.GetCoach(username);
        }
        public List<Student> GetAllStudents()
        {
            return userDAL.GetAllStudents();
        }
        public static Student GetStudent(string username)
        {
            return userDAL.GetStudent(username);
        }

    }
}
