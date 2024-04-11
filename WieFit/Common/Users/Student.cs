using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common.Users
{
    internal class Student : User
    {
        public Student(string _username, string _name, string _email, string _adress, string _phoneNumber, int _age, char _gender)
            : base (_username, _name, _email, _adress, _phoneNumber, _age, _gender)
        { }

        public bool CreateUser(string _password)
        {
            return userDAL.CreateUser(this, _password, 'S');
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
