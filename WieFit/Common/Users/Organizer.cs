using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace WieFit.Common.Users
{
    internal class Organizer : User
    {

        public Organizer(string _username, string _name, string _email, string _adress, string _phoneNumber, int _age, char _gender) :
            base (_username, _name, _email, _adress, _phoneNumber, _age, _gender)
        {}

        public bool CreateUser(string _password)
        {
            return database.CreateUser(this, _password, 'O');
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }

        public bool CreateActivity(Activity activity)
        {
            return database.CreateActivity(activity);
        }
        public bool ActivityInPlanning(PlannedActivity plannedactivity, Planning planning)
        {
            return database.AddActivityToPlanning(plannedactivity, planning);
        }
        public List<Activity> GetAllActivities()
        {
            return database.GetAllActivities();
        }
    }
}
