using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using WieFit.Common.DAL;

namespace WieFit.Common.Users
{
    internal class Organizer : User
    {
        public Organizer(string _username, string _name, string _email, string _address, string _phoneNumber, int _age, char _gender) :
            base (_username, _name, _email, _address, _phoneNumber, _age, _gender)
        {}

        public bool CreateUser(string _password)
        {
            return userDAL.CreateUser(this, _password, 'O');
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }
        public static Organizer ConverToOrganizer(User user)
        {
            return new Organizer(user.Username, user.Name, user.Email, user.Address, user.PhoneNumber, user.Age, user.Gender);
        }
    }
}
