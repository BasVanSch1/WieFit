﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
