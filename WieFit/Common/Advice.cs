using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using WieFit.Common.Users;

namespace WieFit.Common
{
    internal class Advice
    {
        public int Id { get;private set; }
        public string Description {get; private set; }
        public Coach? coach { get; private set; }
        public Student? student { get; private set; }
        public Advice(int _Id, string _Description, Coach _coach, Student _Student)
        {
            Id = _Id;
            Description = _Description;
            coach = _coach;
            student = _Student;
        }
        public Advice(string _Description, Coach _coach, Student _Student)
        { 
            Description = _Description;
            coach = _coach;
            student = _Student;
        }
        public Advice(int _Id, string _Description, Coach _coach)
        {
            Id = _Id;
            Description = _Description;
            coach = _coach;
        }
    }
}
