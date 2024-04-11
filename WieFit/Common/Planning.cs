using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

namespace WieFit.Common
{
    internal class Planning
    {
        protected readonly PlanningDAL planningDAL = PlanningDAL.Instance;
        public List<PlannedActivity> Activities { get; set; }

        public Planning(List<PlannedActivity> activities)
        {
            Activities = activities;
        }

        public Planning()
        {
            Activities = new List<PlannedActivity>();
        }
    }
}
