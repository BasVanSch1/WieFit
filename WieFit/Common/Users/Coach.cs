using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

namespace WieFit.Common.Users
{
    internal class Coach : User
    {
        private static readonly UserDAL userDAL = UserDAL.Instance;
        public Coach(string _username, string _name, string _email, string _address, string _phoneNumber, int _age, char _gender) :
            base (_username, _name, _email, _address, _phoneNumber, _age, _gender)
        {}

        public bool CreateUser(string _password)
        {
            return userDAL.CreateUser(this, _password, 'C');
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }

        public static Coach? GetCoach(string username)
        {
            return userDAL.GetCoach(username);
        }
        public List<Student> GetAllStudents() 
        {
            return userDAL.GetAllStudents();
        }
        public static Student GetStudent(string username)
        {
            return userDAL.GetStudent(username);
        }
        public bool GiveAdvise(Advice advice)
        {
            return userDAL.GiveAdvise(advice);
        }
    }
}
