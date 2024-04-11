using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common.DAL
{
    internal class PlanningDAL : DAL
    {
        private static readonly PlanningDAL instance = new PlanningDAL();

        public static PlanningDAL Instance { get { return instance; } }

        static PlanningDAL() { }
        private PlanningDAL() { }
    }
}
