using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common.Users
{
    internal class Organizer : User
    {

        public Organizer(string _username, string _name, string _email, string _adress, string _phoneNumber, int _age, char _gender) :
            base (_username, _name, _email, _adress, _phoneNumber, _age, _gender)
        {}

        public override bool CreateUser(string _password)
        {
            return database.CreateUser(this, _password, 'O');
        }

        public override bool SignIn()
        {
            throw new NotImplementedException();
        }

        public override bool SignOut()
        {
            throw new NotImplementedException();
        }

        public override bool UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
