using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.DAL
{
    internal class DAL
    {
        private static readonly DAL instance = new DAL();
        private readonly string connectionString = "Data Source=.;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";

        public static DAL Instance { get { return instance; } }

        static DAL() { }
        private DAL() { }
    }
}
