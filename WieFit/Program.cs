using System.Diagnostics;
using WieFit.Common.Users;

namespace WieFit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Common.Activity a = new Common.Activity("naam","beschrijving");
            Organisator o = new Organisator("username","naam","email","adress","telefoon", 1, 'M') ;
            if (o.CreateActivity(a))
            {
                Console.Write("gelukt!");
            }
        }
    }
}
