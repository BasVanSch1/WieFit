using WieFit.Common.Users;

namespace WieFit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateUser();
        }

        static void CreateUser()
        {
            Console.Write("Enter your Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter your Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter your Adress: ");
            string adress = Console.ReadLine();

            Console.Write("Enter your Phonenumber: ");
            string phonenumber = Console.ReadLine();

        }
    }
}
