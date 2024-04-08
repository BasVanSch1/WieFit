using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WieFit.Common
{
    internal class Organisator : User
    {

        public Organisator(string _username, string _name, string _email, string _adress, string _phoneNumber, int _age, char _gender) : 
            base (_username, _name, _email, _adress, _phoneNumber, _age, _gender)
        {
        }

        public bool CreateActivity(Activity activity)
        {
            return database.CreateActivity(activity);
        }
    }
}
