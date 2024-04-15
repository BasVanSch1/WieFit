using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Azure;
using Azure.Core;
using Microsoft.Identity.Client;
using WieFit.Common;
using WieFit.Common.Users;

namespace WieFit
{
    internal class Program
    {
        static LoginManager? loginManager = null;
        static User? LoggedInUser = null;

        static string menuHeader = 
            @"
            ==========================================
                __          ___      ______   _   
                \ \        / (_)    |  ____(_) |  
                 \ \  /\  / / _  ___| |__   _| |_ 
                  \ \/  \/ / | |/ _ \  __| | | __|
                   \  /\  /  | |  __/ |    | | |_ 
                    \/  \/   |_|\___|_|    |_|\__|
            ==========================================";

        static void Main(string[] args)
        {
            Menu();

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
            Console.Write("What location would you like to delete?(Enter location id)");
            int locationid = Convert.ToInt32(Console.ReadLine());
            
        }
        static void CreateActivity()
        {
            Organizer O = new Organizer("username", "name", "mail", "adress", "telefoonnummer", 0, 'M');
            Console.Write("Enter Activity name...");
            string Name = Console.ReadLine();

            Console.Write("Enter Activity Description");
            string Description = Console.ReadLine();

            Common.Activity activity = new Common.Activity(Name, Description);
            if (O.CreateActivity(activity))
            {
                Console.Write("Succes");
            }
            else
            {
                Console.Write("Failed");
            }
        }

        static void GetPlannedActivitiesFromLocation()
        {
            Console.WriteLine("Please enter location id");
            int locationId = Convert.ToInt32(Console.ReadLine());
            List<PlannedActivity> plannedActivities = LoggedInUser.GetPlannedActivitiesFromLocationId(locationId);
            foreach (PlannedActivity plannedActivity in plannedActivities)
            {
                Console.WriteLine($"Activity name = {plannedActivity.Name}");
                Console.WriteLine($"Activity description = {plannedActivity.Description}");
                Console.WriteLine($"Coach name = {plannedActivity.Coach.Name}");
                Console.WriteLine($"Start datatime = {plannedActivity.StartTime}");
                Console.WriteLine($"End datetime = {plannedActivity.EndTime}");
            }
            Console.ReadLine();
        }
        
        static void Login()
        {
            if (loginManager == null)
            {
                loginManager = new LoginManager();
            }
            bool loggedIn = LoggedInUser != null;

            while (!loggedIn)
            {
                Console.Clear();
                Console.WriteLine(menuHeader + "\n");
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
                }
                else
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
            List<Activity>? activities = O.GetAllActivities();
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
        static void AddResult()
        {
            if (LoggedInUser == null)
            {
                return;
            }

            Activity activity = Activity.GetActivity(1);

            Console.Write("When is this result made? enter(YYYY/MM/DD HH:MM:SS): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid Date...");
                Console.Write("When is this result made? enter(YYYY/MM/DD HH:MM:SS): ");
            }

            Console.Write("Add result description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Result value (only integers) : ");
            float result;
            while (!float.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid value...");
                Console.WriteLine("Result value (only integers) : ");
            }

            Result newresult = new Result(date, description, result);

            if (newresult.AddResult(LoggedInUser, activity))
            {
                Console.WriteLine("SUCCESS!");
            }
            else
            {
                Console.WriteLine("FAILED...");
            }
        }
        static void GetAllLocations()
        {
            Organizer O = new Organizer("Organisator", "name", "mail", "address", "telefoonnummer", 0, 'M');
            List<Location>? locations = O.GetAllLocations();
            if (locations == null)
            {
                Console.WriteLine("There are no Locations available.");
                return;
            }
            else
            {
                foreach (Location l in locations)
                {
                    Console.WriteLine($"Locationid: {l.Id}| name: {l.Name} | address: {l.Address} | postalcode: {l.Postalcode} | city: {l.City} | country: {l.Country}");
                }
            }
        }
        static void PlanActivity()
        {
            Organizer O = new Organizer("Organisator", "name", "mail", "address", "telefoonnummer", 0, 'M');

            GetAllLocations();

            int lId;

            Console.Write("Enter locationId: ");
            while (!Int32.TryParse(Console.ReadLine(), out lId))
            {
                Console.WriteLine("Please Enter correct Id");
                Console.Write("Enter locationId: ");
            }
            Location location = O.GetLocation(lId);

            GetAllActivities();

            Console.Write("Select Activity on Id: ");

            int aId;
            DateTime starttime;
            DateTime endtime;

            while (!Int32.TryParse(Console.ReadLine(), out aId))
            {
                Console.WriteLine("Please enter a correct Id..");
                Console.Write("Select Activity on Id: ");
            }

            Activity activity = Activity.GetActivity(aId);

            Console.Write("Enter starttime in format(YYYY-MM-DD HH:MM:SS) : ");
            while (!DateTime.TryParse(Console.ReadLine(), out starttime))
            {
                Console.WriteLine("Please enter a correct Date.");
                Console.Write("Enter starttime: ");
            }

            Console.Write("Enter endtime in format(YYYY-MM-DD HH:MM:SS) : ");
            while (!DateTime.TryParse(Console.ReadLine(), out endtime))
            {
                Console.WriteLine("Please enter a correct Date.");
                Console.Write("Enter endtime: ");
            }
            GetAllCoaches();

            Console.Write("Enter coach username: ");
            string username = Console.ReadLine();
            Coach coach = Coach.GetCoach(username);

            PlannedActivity plannedactivity = new PlannedActivity(aId, activity.Name, activity.Description, starttime, endtime, coach);
            if (O.PlanActivity(plannedactivity, location))
            {
                Console.WriteLine("Succes");
            }
            else
            {
                Console.WriteLine("Failed");
            }
            Console.WriteLine(plannedactivity);
        }
        static void GetAllCoaches()
        {
            Organizer O = new Organizer("Organisator", "name", "mail", "address", "telefoonnummer", 0, 'M');
            Console.WriteLine("Coaches");
            if (O.GetAllCoaches() == null)
            {
                Console.WriteLine("There are no coaches");
            }
            else
            {
                foreach (Coach c in O.GetAllCoaches())
                {
                    Console.WriteLine($"username: {c.Username}| name: {c.Name}| email: {c.Email}| address: {c.Address}| phonenumber: {c.PhoneNumber}| age:{c.Age}| gender:{c.Gender} ");
                }
            }
        }
        static void GetAllStudents()
        {
            Organizer O = new Organizer("Organisator", "name", "mail", "address", "telefoonnummer", 0, 'M');
            List<Student> students = O.GetAllStudents();
            Console.WriteLine("Students: ");
            if (students == null)
            {
                Console.WriteLine("there are no students");
            }
            else
            {
                foreach(Student s in students)
                {
                    Console.WriteLine($"username: {s.Username}| name: {s.Name}| email: {s.Email}| address: {s.Address}| phonenumber: {s.PhoneNumber}| age:{s.Age}| gender:{s.Gender} ");
                }
            }
        }
        static void GetCoach()
        {
            Console.Write("Enter coach username: ");
            string username = Console.ReadLine();
            Coach? c = Coach.GetCoach(username); 
            if (c == null) {
                Console.WriteLine("coach is null");
            }
            else {
                Console.WriteLine($"username: {c.Username}| name: {c.Name}| email: {c.Email}| address: {c.Address}| phonenumber: {c.PhoneNumber}| age:{c.Age}| gender:{c.Gender}");
            }
        }
        static void GetStudent()
        {
            Console.Write("Enter student username: ");
            string username = Console.ReadLine();
            Student? s = Coach.GetStudent(username);
            if (s == null)
            {
                Console.WriteLine("coach is null");
            }
            else
            {
                Console.WriteLine($"username: {s.Username}| name: {s.Name}| email: {s.Email}| address: {s.Address}| phonenumber: {s.PhoneNumber}| age:{s.Age}| gender:{s.Gender}");
            }
        }
        static void GiveAdvise()
        {
            Student? student = null;
            Coach? coach = null;
            GetAllStudents();
            Console.Write("Enter student username: ");
            string Susername = Console.ReadLine();
            student = Coach.GetStudent(Susername);
            while (student == null)
            {
                Console.WriteLine("Please enter correct Username");
                Console.Write("Enter student username: ");
                Susername = Console.ReadLine();
                student = Coach.GetStudent(Susername);
            }
            GetAllCoaches();
            Console.Write("Enter Coach Username");
            string Cusername = Console.ReadLine();
            coach = Organizer.GetCoach(Cusername);
            while( coach == null )
            {
                Console.WriteLine("Please enter correct Username");
                Console.Write("Enter Coach Username");
                Cusername = Console.ReadLine();
                coach = Organizer.GetCoach(Cusername);
            }

            Console.Write("Type your advise here: ");
            string Advice = Console.ReadLine();

            Advice advice = new Advice(Advice,coach, student);
            if (coach.GiveAdvise(advice))
            {
                Console.WriteLine("Succes!");
            }
        }
        static void GetAdvice()
        {
            Student s = null;
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            s = Coach.GetStudent(username);
            while (s == null)
            {
                Console.WriteLine("Please enter a correct username");
                Console.Write("Enter Username: ");
                username = Console.ReadLine();
                s = Coach.GetStudent(username);
            }
            List<Advice> advices = null;
            advices = s.GetAdvice(username);
            if (advices == null)
            {
                Console.WriteLine("there are no advices");
            }
            else
            {
                Console.WriteLine("Advice: ");
                foreach (Advice a in advices)
                {
                    Console.WriteLine($"Id: {a.Id}|Advice: {a.Description}| Coach: {a.coach.Name}");
                }
            }
            
            
        }
        static void Logout()
        {
            if (LoggedInUser == null)
            {
                Console.WriteLine("There is no logged in user..");
                return;
            }

            LoggedInUser = null;
            Console.WriteLine("You have been logged out.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void Menu()
        {
            bool inMenu = true;

            Dictionary<int, KeyValuePair<string, Action>> loginMenu = new()
            {
                [1] = new KeyValuePair<string, Action>("Login", Login),
                [2] = new KeyValuePair<string, Action>("Create account", CreateAccount)
            };

            Dictionary<int, KeyValuePair<string, Action>> menuItems = new()
            {
                [99] = new KeyValuePair<string, Action>("Logout", Logout),
                [1] = new KeyValuePair<string, Action>("Get all locations", GetAllLocations),
                [2] = new KeyValuePair<string, Action>("Add result", AddResult),
            };

            while (LoggedInUser == null)
            {
                Console.Clear();
                Console.WriteLine(menuHeader);

                foreach (KeyValuePair<int, KeyValuePair<string, Action>> pair in loginMenu)
                {
                    Console.WriteLine($"{pair.Key}) {pair.Value.Key}");
                }

                int choice = -1;
                Console.Write("Enter a menu item number: ");
                while (!Int32.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                    Console.Write("Enter a menu item number: ");
                }

                if (loginMenu.TryGetValue(choice, out KeyValuePair<string, Action> action))
                {
                    action.Value();
                }
                else
                {
                    Console.WriteLine("That option does not exist. Try again.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine(menuHeader);

                foreach (KeyValuePair<int, KeyValuePair<string, Action>> pair in menuItems)
                {
                    Console.WriteLine($"{pair.Key}) {pair.Value.Key}");
                }

                int choice = -1;
                Console.Write("Enter a menu item number: ");
                while (!Int32.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                    Console.Write("Enter a menu item number: ");
                }
                
                if (menuItems.TryGetValue(choice, out KeyValuePair<string, Action> action))
                {
                    action.Value();
                } else
                {
                    Console.WriteLine("That option does not exist. Try again.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        static void CreateAccount()
        {
            if (LoggedInUser != null)
            {
                Console.WriteLine("Cannot create an account when you are already logged in.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (loginManager == null)
            {
                loginManager = new LoginManager();
            }

            Console.Write("Enter your username: ");
            string? username = Console.ReadLine();

            while (username == null || username.Length <= 0)
            {
                Console.WriteLine("Your username cannot be empty.");
                Console.Write("Enter your username: ");
                username = Console.ReadLine();
            }

            while (!loginManager.CheckUsernameFree(username))
            {
                Console.WriteLine("That username is already taken. Please choose another.");
                Console.Write("Enter your username: ");
                username = Console.ReadLine();
            }

            Console.Write("Enter your new password: ");
            string? password = Console.ReadLine();

            while (password == null || password.Length <= 0)
            {
                Console.WriteLine("Your password cannot be empty.");
                Console.Write("Enter your new password: ");
                password = Console.ReadLine();
            }

            Console.Write("Validate your password: ");
            string? validatepassword = Console.ReadLine();

            while (password != validatepassword)
            {
                Console.WriteLine("Passwords do not match! try again.");

                Console.Write("Enter your new password: ");
                password = Console.ReadLine();

                Console.Write("Validate your password: ");
                validatepassword = Console.ReadLine();
            }

            Console.Write("Enter your real name: ");
            string? name = Console.ReadLine();

            while (name == null || name.Length <= 0)
            {
                Console.WriteLine("Your name cannot be empty.");
                Console.Write("Enter your real name: ");
                name = Console.ReadLine();
            }

            Console.Write("Enter your email: ");
            string? email = Console.ReadLine();

            while (email == null || email.Length <= 0)
            {
                Console.WriteLine("Your email cannot be empty.");
                Console.Write("Enter your email: ");
                email = Console.ReadLine();
            }

            Console.Write("Enter your address: ");
            string? address = Console.ReadLine();

            while (address == null || address.Length <= 0)
            {
                Console.WriteLine("Your address cannot be empty.");
                Console.Write("Enter your address: ");
                address = Console.ReadLine();
            }

            Console.Write("Enter your phonenumber: ");
            string? phonenumber = Console.ReadLine();

            while (phonenumber == null || phonenumber.Length <= 0 || phonenumber.Any(x => char.IsLetter(x)))
            {
                Console.WriteLine("Invalid input. Try again.");
                Console.Write("Enter your phonenumber: ");
                phonenumber = Console.ReadLine();
            }

            Console.Write("Enter your Age: ");
            int age;
            while (!Int32.TryParse(Console.ReadLine(), out age))
            {
                Console.WriteLine("Only integers are allowed [00-99]. Please try again...");
                Console.Write("Enter your Age: ");
            }

            Console.Write("Enter your Gender (M = Male, F = Female, O = Other): ");
            char gender = Console.ReadLine().ToCharArray().First();
            while (char.ToUpper(gender) != 'F' && char.ToUpper(gender) != 'M' && char.ToUpper(gender) != 'O')
            {
                Console.WriteLine("Invalid input, enter either 'F', 'M' or 'O'.");
                Console.Write("Enter your Gender (M = Male, F = Female, O = Other): ");
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

            switch (char.ToUpper(accounttype))
            {
                case 'C':
                    {
                        Coach user = new Coach(username, name, email, address, phonenumber, age, gender);
                        if (user.CreateUser(password))
                        {
                            Console.WriteLine("Account created!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to create account!");
                        }
                        break;
                    }
                case 'S':
                    {
                        Student user = new Student(username, name, email, address, phonenumber, age, gender);
                        if (user.CreateUser(password))
                        {
                            Console.WriteLine("Account created!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to create account!");
                        }
                        break;
                    }
                case 'O':
                    {
                        Organizer user = new Organizer(username, name, email, address, phonenumber, age, gender);
                        if (user.CreateUser(password))
                        {
                            Console.WriteLine("Account created!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to create account!");
                        }
                        break;
                    }
                default:
                    Console.WriteLine("uh-oh, dit zou niet mogen gebeuren :P");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }
    }
}
