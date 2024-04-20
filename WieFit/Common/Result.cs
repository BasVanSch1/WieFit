using WieFit.Common.DAL;
using WieFit.Common.Users;

namespace WieFit.Common
{
    internal class Result
    {
        private readonly ResultDAL resultDAL = ResultDAL.Instance;

        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public decimal Value { get; private set; }
        public Activity Activity { get; private set; }

        public Result(DateTime _date, string _description, decimal _value, Activity activity)
        {

            Date = _date;
            Description = _description;
            Value = _value;
            Activity = activity;
        }

        public bool AddResult(User user)
        {
            return resultDAL.AddResult(this, user);
        }
    }
}
