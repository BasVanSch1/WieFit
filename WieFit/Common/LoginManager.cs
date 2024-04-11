using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;
using WieFit.Common.Users;

namespace WieFit.Common
{
    internal class LoginManager
    {
        private readonly DAL.DAL database = DAL.DAL.Instance;

        public LoginManager()
        {
             
        }

        private bool CheckPassword(string username, string password)
        {
            return database.CheckPassword(username, password);
        }

        public User? GetUser(string username, string password)
        {
            if (CheckPassword(username, password))
            {
                User? user = database.GetUser(username, password);
                if (user != null)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
