using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.Users;

namespace WieFit.Common.DAL
{
    internal abstract class DAL
    {
        protected readonly string connectionString = @"Data Source=.;Initial Catalog=WieFit;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";

        public DAL() { }
      
    }
}
