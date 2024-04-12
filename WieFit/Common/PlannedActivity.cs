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

        public PlannedActivity(string _Name, string _Description,DateTime _StartTime,DateTime _EndTime, Coach _Coach) : base(_Name, _Description)
        {
            StartTime= _StartTime;
            EndTime= _EndTime;
            Coach = _Coach;
        }

        public override string ToString()
        {
            return $"Id: {Id}| Name: {Name}| Description {Description}| StartTime: {StartTime}| Endtime: {EndTime}| CoachName: {Coach.Name}";
        }
    }
}