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

        public bool CheckUsernameFree(string username)
        {
            return userDAL.CheckUsernameFree(username);
        }
    }
}
