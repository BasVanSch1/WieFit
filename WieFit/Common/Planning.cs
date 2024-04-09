using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common
{
    internal class Planning
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public List<PlannedActivity> Activities { get; set; }

        public Planning(int id, bool isActive, List<PlannedActivity> activities)
        {
            Id = id;
            IsActive = isActive;
            Activities = activities;
        }

        public Planning()
        {
            IsActive = false;
            Activities = new List<PlannedActivity>();
        }
    }
}
