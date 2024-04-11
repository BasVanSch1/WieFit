using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.Users;
using WieFit.Common.DAL;

namespace WieFit.Common
{
    internal class Result
    {
        private readonly ResultDAL resultDAL = ResultDAL.Instance;
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public float Value { get; private set; }

        public Result(DateTime _date, string _description, float _value)
        {

            Date = _date;
            Description = _description;
            Value = _value;
        }

        public bool AddResult(User user, Activity activity)
        {
            return resultDAL.AddResult(this, user, activity);
        }
    }
}
