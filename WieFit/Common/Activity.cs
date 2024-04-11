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
        private readonly ActivityDAL activityDAL = ActivityDAL.Instance;
        
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

        public Activity GetActivity(int id)
        {
            return activityDAL.GetActivity(id);
        }
    }
    
}
