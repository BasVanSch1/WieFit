using WieFit.Common.Users;

namespace WieFit.Common
{
    internal class PlannedActivity : Activity
    {
        public DateTime StartTime { get; private set; } 
        public DateTime EndTime { get; private set; }
        public Coach Coach { get; private set; }
        
        public PlannedActivity(int _Id, string _Name, string _Description, DateTime _StartTime, DateTime _EndTime, Coach _Coach) : base(_Id, _Name, _Description)
        {
            StartTime = _StartTime;
            EndTime = _EndTime;
            Coach = _Coach;
        }

        public PlannedActivity(string _Name, string _Description,DateTime _StartTime, DateTime _EndTime, Coach _Coach) : base(_Name, _Description)
        {
            StartTime= _StartTime;
            EndTime= _EndTime;
            Coach = _Coach;
        }

        public static PlannedActivity? CreatePlannedActivity(Activity _activity, DateTime _starttime, DateTime _endtime, Coach _coach, int _locationid)
        {
            return activityDAL.PlanActivity(_activity, _starttime, _endtime, _coach, _locationid);
        }
    }
}