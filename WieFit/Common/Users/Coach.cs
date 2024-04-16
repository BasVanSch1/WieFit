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

        public static Coach ConvertToCoach(User user)
        {
            return new Coach(user.Username, user.Name, user.Email, user.Address, user.PhoneNumber, user.Age, user.Gender);
        }

        public List<Student>? GetStudents() 
        {
            return userDAL.GetStudentsFromCoach(this);
        }
        public bool GiveAdvice(Student student, Advice advice)
        {
            return userDAL.GiveAdvise(advice);
        }

        public static List<Coach>? GetAllCoaches()
        {
            return userDAL.GetAllCoaches();
        }
    }
}
