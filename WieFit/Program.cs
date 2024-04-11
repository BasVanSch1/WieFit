﻿using System.Runtime.InteropServices;
using Azure.Core;
using WieFit.Common;
using WieFit.Common.Users;

namespace WieFit
{
    internal class Program
    {
        static LoginManager? loginManager = null;
        static User? LoggedInUser = null;

        static void Main(string[] args)
        {
            while (LoggedInUser == null)
            {
                Login();
            }
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

        static void AddLocation() 
        {
            Console.Write("Enter location name: ");
            string _locationname = Console.ReadLine();

            Console.Write("Enter location adress: ");
            string _locationadress = Console.ReadLine();

            Console.Write("Enter postalcode: ");
            string _postalcode = Console.ReadLine();

            Console.Write("Enter city: ");
            string _city = Console.ReadLine();

            Console.Write("Enter country: ");
            string _country = Console.ReadLine();
            Location location = new Location(_locationname, _locationadress, _postalcode, _city, _country);
            
            if (location.AddLocation())
            {
                Console.WriteLine("SUCCESS@?@");
            } else
            {
                Console.WriteLine("FAILED..");
            }
            
        }

        static void DeleteLocation() 
        {
        }

        static void CreatePlanning()
        {
            Planning planning = new Planning();
            planning.CreatePlanning();
        }

        static void CreateActivity()
        {
            Organizer O = new Organizer("username","name","mail","adress","telefoonnummer", 0,'M');
            Console.Write("Enter Activity name...");
            string Name = Console.ReadLine();

            Console.Write("Enter Activity Description");
            string Description = Console.ReadLine();

            Common.Activity activity = new Common.Activity(Name,Description);
            if (O.CreateActivity(activity))
            {
                Console.Write("Succes");
            }
            else
            {
                Console.Write("Failed");
            }
        } 

        static void Login()
        {
            if (loginManager == null)
            {
                loginManager = new LoginManager();
            }
            bool loggedIn = false;

            while (!loggedIn)
            {
                Console.Clear();
                Console.Write("Please enter your username: ");
                string? username = Console.ReadLine();
                while (username == null || username.Length <= 0)
                {
                    Console.Write("Your username cannot be empty, please try again...");
                    Console.Write("Please enter your username: ");
                    username = Console.ReadLine();
                }

                Console.Write("Please enter your password: ");
                string? password = Console.ReadLine();
                while (password == null || password.Length <= 0)
                {
                    Console.Write("Your password cannot be empty, please try again...");
                    Console.Write("Please enter your password: ");
                    password = Console.ReadLine();
                }

                User? user = loginManager.GetUser(username, password);

                if (user != null)
                {
                    Console.WriteLine("Succesfully logged in.");
                    LoggedInUser = user;
                    loggedIn = true;
                } else
                {
                    Console.WriteLine("Login failed. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
      
        static void GetAllActivities()
        {
            Organizer O = new Organizer("username", "name", "email", "adress", "telefoonnummer", 0, 'M');
            List<Activity> activities = new List<Activity>(O.GetAllActivities());
            Console.WriteLine("Activities:");
            foreach (Activity a in activities)
            {
                Console.WriteLine($"Id: {a.Id} Name:{a.Name} Description: {a.Description} ");
            }
        }
        
        static void GetActivity()
        {
            Console.Write("Enter an activity id:");
            int id = Convert.ToInt32(Console.ReadLine());
            Activity activity = Activity.GetActivity(id);
            if (activity == null)
            {
                Console.WriteLine($"No Activity was found with id {id}");
                return;
            }
            Console.WriteLine($"Id: {activity.Id} Name:{activity.Name} Description: {activity.Description} ");
        }
    }
}
