using System.Diagnostics;
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

            Console.Write("Enter your Age: ");
            int age;
            while (!Int32.TryParse(Console.ReadLine(), out age))
            {
                Console.WriteLine("Only integers are allowed [00-99]. Please try again...");
                Console.Write("Enter your Age: ");
            }

            Console.Write("Enter your Gender (M/F/O): ");
            char gender = Console.ReadLine().ToCharArray().First();
            while (char.ToUpper(gender) != 'F' && char.ToUpper(gender) != 'M' && char.ToUpper(gender) != 'O')
            {
                Console.WriteLine("Invalid input, enter either 'F', 'M' or 'O'.");
                Console.Write("Enter your Gender (M/F/O): ");
                gender = Console.ReadLine().ToCharArray().First();
            }

            Console.Write("Enter your AccountType (C = Coach, S = Student, O = Organizer): ");
            char accounttype = Console.ReadLine().ToCharArray().First();
            while (char.ToUpper(accounttype) != 'C' && char.ToUpper(accounttype) != 'S' & char.ToUpper(accounttype) != 'O')
            {
                Console.WriteLine("Invalid input, enter either 'C', 'S' or 'O'.");
                Console.Write("Enter your AccountType (C = Coach, S = Student, O = Organizer): ");
                accounttype = Console.ReadLine().ToCharArray().First();
            }

            Console.Write("Please enter a password for your account: ");
            string password = Console.ReadLine();

            switch (char.ToUpper(accounttype))
            {
                case 'C':
                    {
                        Coach user = new Coach(username, name, email, adress, phonenumber, age, gender);
                        if (user.CreateUser(password))
                        {
                            Console.WriteLine("Success!");
                        }
                        else
                        {
                            Console.WriteLine("Failed!");
                        }
                        break;
                    }
                case 'S':
                    {
                        Student user = new Student(username, name, email, adress, phonenumber, age, gender);
                        if (user.CreateUser(password))
                        {
                            Console.WriteLine("Success!");
                        }
                        else
                        {
                            Console.WriteLine("Failed!");
                        }
                        break;
                    }
                case 'O':
                    {
                        Organizer user = new Organizer(username, name, email, adress, phonenumber, age, gender);
                        if (user.CreateUser(password))
                        {
                            Console.WriteLine("Success!");
                        }
                        else
                        {
                            Console.WriteLine("Failed!");
                        }
                        break;
                    }
                default:
                    Console.WriteLine("uh-oh, dit zou niet mogen gebeuren :P");
                    break;
            }
        }
    }
}
