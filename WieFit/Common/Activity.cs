using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

namespace WieFit.Common
{
    internal class Activity
    {
        private static readonly ActivityDAL activityDAL = ActivityDAL.Instance;
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Activity(int _Id, string _Name,string _Description)
        {
            Id = _Id;
            Name = _Name;
            Description = _Description;
        }
        public Activity(string _Name, string _Description)
        {
            Name = _Name;
            Description = _Description;
        }

        public static Activity? GetActivity(int id)
        {
            return activityDAL.GetActivity(id);
        }

        public static List<Activity>? GetAllActivities()
        {
            return activityDAL.GetAllActivities();
        }

        public static Activity? CreateActivity(string _name, string _description)
        {
            return activityDAL.CreateActivity(_name, _description);
        }
    }
    
}
