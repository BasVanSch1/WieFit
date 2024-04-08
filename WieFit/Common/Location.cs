using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common
{
    internal class Location
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public List<PlannedActivity> Activities { get; set; }

        public Location(int id, bool isActive, List<PlannedActivity> activities)
        {
            Id = id;
            IsActive = isActive;
            Activities = activities;
        }

        public Location()
        {
            IsActive = false;
            Activities = new List<PlannedActivity>();
        }
    }
}
