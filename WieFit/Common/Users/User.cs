﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

namespace WieFit.Common.Users
{
    internal class User
    {
        protected readonly UserDAL userDAL = UserDAL.Instance;

        public string Username { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Adress {  get; private set; }
        public string PhoneNumber {  get; private set; }
        public int Age { get; private set; }
        public char Gender { get; private set; }

        public User(string _username, string _name, string _email, string _adress, string _phoneNumber, int _age, char _gender)
        {
            Username = _username;
            Name = _name;
            Email = _email;
            Adress = _adress;
            PhoneNumber = _phoneNumber;
            Age = _age;
            Gender = _gender;
        }
    }
}
