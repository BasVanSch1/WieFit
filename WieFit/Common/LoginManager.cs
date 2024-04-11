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
        private readonly UserDAL userDAL = UserDAL.Instance;

        public LoginManager()
        {
             
        }

        private bool CheckPassword(string username, string password)
        {
            return userDAL.CheckPassword(username, password);
        }

        public User? GetUser(string username, string password)
        {
            if (CheckPassword(username, password))
            {
                User? user = userDAL.GetUser(username, password);
                if (user != null)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
