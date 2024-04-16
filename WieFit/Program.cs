using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Azure;
using Azure.Core;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using WieFit.Common;
using WieFit.Common.Users;
using Activity = WieFit.Common.Activity;

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
            ==========================================
            ";

        static void Main(string[] args)
        {
            Menu();
        }

        static void CreateLocation() 
        {
            Console.Clear();
            Console.WriteLine(menuHeader);

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
                Console.WriteLine(menuHeader);

                Console.Write("Please enter your username: ");
                string? username = Console.ReadLine();
                while (username == null || username.Length <= 0)
                {
                    Console.WriteLine("Your username cannot be empty, please try again...");
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
                    LoggedInUser = user;
                    loggedIn = true;
                    Console.WriteLine("Succesfully logged in.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Login failed. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        static void AddResult()
        {
            if (LoggedInUser == null)
            {
                return;
            }

            Console.Clear();
            Console.WriteLine(menuHeader);

            List<Activity>? activityList = Activity.GetAllActivities();

            if (activityList == null || activityList.Count == 0)
            {
                Console.WriteLine("There are no activities registered in the system. Ask an organizer to add some!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            
            foreach(Activity act in activityList)
            {
                Console.WriteLine($"ID: {act.Id} | Name: {act.Name} | Description: {act.Description}");
            }

            Console.Write("Enter the activity ID you which to add a result for: ");
            int activityid = -1;
            while (!Int32.TryParse(Console.ReadLine(), out activityid))
            {
                Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                Console.Write("Enter the activity ID you which to add a result for:");
            }

            Activity? activity = Activity.GetActivity(activityid);
            while (activity == null)
            {
                Console.WriteLine("That activity does not exist. Try again.");
                Console.Write("Enter the activity ID you which to add a result for: ");

                while (!Int32.TryParse(Console.ReadLine(), out activityid))
                {
                    Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                    Console.Write("Enter the activity ID you which to add a result for:");
                }

                activity = Activity.GetActivity(activityid); // HEEL slecht, telkens naar de database vragen voor de activity. maarja, wat is een 'beetje' technical debt nou?
            }

            Console.Write("When is this result made? (YYYY/MM/DD HH:MM:SS): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid Date...");
                Console.Write("When is this result made? (YYYY/MM/DD HH:MM:SS): ");
            }

            Console.Write("Add result description: ");
            string? description = Console.ReadLine();
            while (description == null || description.Length <= 0)
            {
                Console.WriteLine("Description cannot be empty. Try again.");
                Console.Write("Add result description: ");
                description = Console.ReadLine();
            }

            Console.Write("Result value (only integers) : ");
            decimal result;
            while (!decimal.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid value...");
                Console.Write("Result value (only integers) : ");
            }

            Result newresult = new Result(date, description, result, activity);

            if (newresult.AddResult(LoggedInUser))
            {
                Console.WriteLine("SUCCESS!");
            }
            else
            {
                Console.WriteLine("FAILED...");
            }
        }
        static void GiveAdvice()
        {
            Console.Clear();
            Console.WriteLine(menuHeader);

            if (LoggedInUser == null)
            {
                return;
            }

            if (!(LoggedInUser.Type == 'C' || LoggedInUser.Type == 'c' || LoggedInUser.Type == 'O' || LoggedInUser.Type == 'o'))
            {
                Console.WriteLine("You do not have permission to use this function.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Coach coach = Coach.ConvertToCoach(LoggedInUser);
            List<Student>? students = coach.GetStudents();
            Dictionary<string, Student> selectionList = new Dictionary<string, Student>();

            if (students == null)
            {
                Console.WriteLine("You have no students. Ask an organizer to add some.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            foreach (Student student in students)
            {
                string _gender = "undefined";
                switch (student.Gender)
                {
                    case 'F' or 'f':
                        _gender = "Female";
                        break;
                    case 'M' or 'm':
                        _gender = "Male";
                        break;
                    case 'O' or 'o':
                        _gender = "Other";
                        break;
                }

                selectionList.Add(student.Username, student);
                Console.WriteLine($"Username: {student.Username} | Name: {student.Name} | Age: {student.Age} | Gender: {_gender}");
            }
            
            Console.WriteLine("Select a student (username)");
            string? selection = Console.ReadLine();
            while (selection == null || selection.Length <= 0)
            {
                Console.WriteLine("Username cannot be empty. Please try again.");
                Console.WriteLine("Select a student (username)");
                selection = Console.ReadLine();
            }

            while (!selectionList.ContainsKey(selection))
            {
                Console.WriteLine("That student does not exist. Pleas try again.");

                Console.WriteLine("Select a student (username)");
                selection = Console.ReadLine();
                while (selection == null || selection.Length <= 0)
                {
                    Console.WriteLine("Username cannot be empty. Please try again.");
                    Console.WriteLine("Select a student (username)");
                    selection = Console.ReadLine();
                }
            }

            Student selectedStudent = selectionList[selection];

            Console.WriteLine($"Enter your advice to {selectedStudent.Name}: ");
            string? adviceText = Console.ReadLine();
            while (adviceText == null || adviceText.Length <= 0)
            {
                Console.WriteLine("Advice cannot be empty. Please try again.");
                Console.WriteLine($"Enter your advice to {selectedStudent.Name}: ");
                adviceText = Console.ReadLine();
            }

            Advice advice = new Advice(adviceText, coach, selectedStudent);
            if (coach.GiveAdvice(selectedStudent, advice))
            {
                Console.WriteLine($"Succesfully gave advice to {selectedStudent.Name}.");
            } else
            {
                Console.WriteLine("Failed to give advice.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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
                // Student
                // [1] = new KeyValuePair<string, Action>("Register for an activity", RegisterForActivity),
                // [2] = new KeyValuePair<string, Action>("Unregister for an activity", UnregisterForActivity),
                [3] = new KeyValuePair<string, Action>("Look at Advice from Coach", LookupAdvice),
                [4] = new KeyValuePair<string, Action>("Lookup location information", LookupLocation),
                [5] = new KeyValuePair<string, Action>("Add result (activity)", AddResult),
                [6] = new KeyValuePair<string, Action>("Lookup results", LookupResult),
                
                // Coach
                [10] = new KeyValuePair<string, Action>("Give advice to Student", GiveAdvice),

                // Organisator
                [15] = new KeyValuePair<string, Action>("Create activity (template)", CreateActivity),
                [16] = new KeyValuePair<string, Action>("Plan activity", PlanActivity),
                [17] = new KeyValuePair<string, Action>("Create location", CreateLocation),
                [18] = new KeyValuePair<string, Action>("Delete location", DeleteLocation),

                // Everyone
                [99] = new KeyValuePair<string, Action>("Logout", Logout),
            };

            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine(menuHeader);

                while (LoggedInUser == null)
                {
                    foreach (KeyValuePair<int, KeyValuePair<string, Action>> pair in loginMenu)
                    {
                        Console.WriteLine($"{pair.Key}) {pair.Value.Key}");
                    }

                    int loginChoice = -1;
                    Console.Write("Enter a menu item number: ");
                    while (!Int32.TryParse(Console.ReadLine(), out loginChoice))
                    {
                        Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                        Console.Write("Enter a menu item number: ");
                    }

                    if (loginMenu.TryGetValue(loginChoice, out KeyValuePair<string, Action> loginAction))
                    {
                        loginAction.Value();
                    }
                    else
                    {
                        Console.WriteLine("That option does not exist. Try again.");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                }

                Console.Clear();
                Console.WriteLine(menuHeader);

                foreach (KeyValuePair<int, KeyValuePair<string, Action>> pair in menuItems)
                {
                    switch (LoggedInUser.Type)
                    {
                        case 'S' or 's': // show only student items
                            if (pair.Key >= 1 && pair.Key < 10 || pair.Key == 99)
                            {
                                Console.WriteLine($"{pair.Key}) {pair.Value.Key}");
                            }
                            break;
                        case 'C' or 'c': // show only student + coach items
                            if (pair.Key >= 1 && pair.Key < 15 || pair.Key == 99)
                            {
                                Console.WriteLine($"{pair.Key}) {pair.Value.Key}");
                            }
                            break;
                        case 'O' or 'o': // show everything for organizers
                            Console.WriteLine($"{pair.Key}) {pair.Value.Key}");
                            break;
                    }
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
        static void LookupAdvice()
        {
            if (LoggedInUser == null)
            {
                return;
            }    

            Console.Clear();
            Console.WriteLine(menuHeader);

            Student student = Student.ConverToStudent(LoggedInUser);
            List<Advice>? adviceList = student.GetAdvice();

            if (adviceList == null || adviceList.Count == 0)
            {
                Console.WriteLine("You have no advice yet. Ask your coach!");
            } else
            {
                foreach (Advice advice in adviceList)
                {
                    Console.WriteLine(advice);
                }
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        static void LookupResult()
        {
            Console.Clear();
            Console.WriteLine(menuHeader);

            if (LoggedInUser == null)
            {
                return;
            }

            Student? student = Student.ConverToStudent(LoggedInUser);
            List<Result>? resultList = student.GetResults();

            if (resultList == null)
            {
                Console.WriteLine("You have no results yet. Go do an activity and add your results!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("======");

            foreach (Result result in resultList)
            {
                Console.WriteLine(
                    $"""
                    Activity:       {result.Activity.Name}
                    Date:           {result.Date}
                    Description:    {result.Description}
                    Result value:   {result.Value}
                    ======
                    """);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void LookupLocation()
        {
            Console.Clear();
            Console.WriteLine(menuHeader);

            if (LoggedInUser == null)
            {
                return;
            }

            List<Location>? locationList = Location.GetAllLocations();

            if (locationList == null || locationList.Count == 0)
            {
                Console.WriteLine("There are no locations registered in the system. Ask an organizer to add some.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Locaties:");

            foreach (Location loc in locationList)
            {
                Console.WriteLine($"ID: {loc.Id} | Name: {loc.Name} | Address: {loc.Address} | Postalcode: {loc.Postalcode} | City: {loc.City} | Country: {loc.Country}");
            }

            Console.Write("Enter a location ID to lookup more information (enter -1 to cancel):  ");
            int locationid = -1;
            while (!Int32.TryParse(Console.ReadLine(), out locationid))
            {
                Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                Console.Write("Enter a location ID to lookup more information (enter -1 to cancel):  ");
            }

            if (locationid == -1)
            {
                return;
            }

            Location? location = Location.GetLocation(locationid);
            while (location == null)
            {
                Console.WriteLine("That location does not exist. Try again.");
                Console.Write("Enter a location ID to lookup more information (enter -1 to cancel):  ");

                while (!Int32.TryParse(Console.ReadLine(), out locationid))
                {
                    Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                    Console.Write("Enter a location ID to lookup more information (enter -1 to cancel):  ");
                }

                if (locationid == -1) // misschien een method van maken? is eigenlijk copy-paste van hierboven.
                {
                    return;
                }

                location = Location.GetLocation(locationid); // HEEL slecht, telkens naar de database vragen voor de locatie. maarja, wat is een 'beetje' technical debt nou?
            }

            Console.Clear();
            Console.WriteLine(menuHeader);
            Console.WriteLine(
                $"""
                ID:         {location.Id}
                Name:       {location.Name}
                Address:    {location.Address}
                Postalcode: {location.Postalcode}
                City:       {location.City}
                Country:    {location.Country}

                Planned activities:
                ======
                """);

            if (location.PlannedActivities == null || location.PlannedActivities.Count == 0)
            {
                Console.WriteLine("There are no activities planned for this location.");
                Console.WriteLine("======");
            } else
            {
                foreach (PlannedActivity activity in location.PlannedActivities)
                {
                    Console.WriteLine($"""
                        Name:           {activity.Name}
                        Description:    {activity.Description}
                        Start time:     {activity.StartTime}
                        End time:       {activity.EndTime}
                        Coach:          {activity.Coach.Name}
                        ======
                        """);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void CreateActivity()
        {
            Console.Clear();
            Console.WriteLine(menuHeader);

            if (LoggedInUser == null)
            {
                return;
            }

            if (!(LoggedInUser.Type == 'O' || LoggedInUser.Type == 'o'))
            {
                Console.WriteLine("You do not have permission to use this function.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the name of the Activity: ");
            string? activityName = Console.ReadLine();
            while (activityName == null || activityName.Length <= 0)
            {
                Console.WriteLine("The activity name cannot be empty. Please try again.");
                Console.Write("Enter the name of the Activity: ");
                activityName = Console.ReadLine();
            }

            Console.Write("Enter the description of the Activity: ");
            string? activityDescription = Console.ReadLine();
            while (activityDescription == null || activityDescription.Length <= 0)
            {
                Console.WriteLine("The activity description cannot be empty. Please try again.");
                Console.Write("Enter the name of the Activity: ");
                activityDescription = Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine(menuHeader);

            Activity? activity = Activity.CreateActivity(activityName, activityDescription);
            if (activity == null)
            {
                Console.WriteLine("Something went wrong when uploading the activity. try again later.");
            } else
            {
                Console.WriteLine("Activity created!");
                Console.WriteLine(
                    $"""
                    ======
                    Activity ID:            {activity.Id}
                    Activity Name:          {activity.Name}
                    Activity Description:   {activity.Description}
                    ======
                    """);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void PlanActivity()
        {
            Console.Clear();
            Console.WriteLine(menuHeader);

            if (LoggedInUser == null)
            {
                return;
            }

            if (!(LoggedInUser.Type == 'O' || LoggedInUser.Type == 'o'))
            {
                Console.WriteLine("You do not have permission to use this function.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            // Ik HAAT het om het zo te doen, maar ik kan niet zomaar LookupLocation() hier neerzetten........... :'(
            List<Location>? locationList = Location.GetAllLocations();

            if (locationList == null || locationList.Count == 0)
            {
                Console.WriteLine("There are no locations registered in the system. Ask an organizer to add some.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Locaties:");

            foreach (Location loc in locationList)
            {
                Console.WriteLine($"ID: {loc.Id} | Name: {loc.Name} | Address: {loc.Address} | Postalcode: {loc.Postalcode} | City: {loc.City} | Country: {loc.Country}");
            }

            Console.Write("Enter a location ID to plan an activity for (enter -1 to cancel):  ");
            int locationid = -1;
            while (!Int32.TryParse(Console.ReadLine(), out locationid))
            {
                Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                Console.Write("Enter a location ID to plan an activity for (enter -1 to cancel):  ");
            }

            if (locationid == -1)
            {
                return;
            }

            Location? location = Location.GetLocation(locationid);
            while (location == null)
            {
                Console.WriteLine("That location does not exist. Try again.");
                Console.Write("Enter a location ID to plan an activity for  (enter -1 to cancel):  ");

                while (!Int32.TryParse(Console.ReadLine(), out locationid))
                {
                    Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                    Console.Write("Enter a location ID to plan an activity for (enter -1 to cancel):  ");
                }

                if (locationid == -1)
                {
                    return;
                }

                location = Location.GetLocation(locationid); // HEEL slecht, telkens naar de database vragen voor de locatie. maarja, wat is een 'beetje' technical debt nou?
            }

            Console.Clear();
            Console.WriteLine(menuHeader);
            Console.WriteLine(
                $"""
                ID:         {location.Id}
                Name:       {location.Name}
                Address:    {location.Address}
                Postalcode: {location.Postalcode}
                City:       {location.City}
                Country:    {location.Country}

                Planned activities:
                ======
                """);

            if (location.PlannedActivities == null || location.PlannedActivities.Count == 0)
            {
                Console.WriteLine("There are no activities planned for this location.");
                Console.WriteLine("======");
            }
            else
            {
                foreach (PlannedActivity activity in location.PlannedActivities)
                {
                    Console.WriteLine($"""
                        Name:           {activity.Name}
                        Description:    {activity.Description}
                        Start time:     {activity.StartTime}
                        End time:       {activity.EndTime}
                        Coach:          {activity.Coach.Name}
                        ======
                        """);
                }
            }

            List<Activity>? activityList = Activity.GetAllActivities();
            Dictionary<int, Activity>? activityChoices = new Dictionary<int, Activity>();
            Console.WriteLine(
                $"""

                Available Activity Templates:
                ======
                """);

            if (activityList == null  || activityList.Count == 0)
            {
                Console.WriteLine("There are no activity templates registered in the system. Create some first!");
                Console.WriteLine("======");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            foreach (Activity activity in activityList)
            {
                activityChoices.Add(activity.Id, activity);
                Console.WriteLine(
                    $"""
                    Activity ID:            {activity.Id}
                    Activity Name:          {activity.Name}
                    Activity Description:   {activity.Description}
                    ======
                    """);
            }

            Console.Write("Select an activity ID: ");
            int activityid;
            while (!Int32.TryParse(Console.ReadLine(), out activityid))
            {
                Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                Console.Write("Select an activity ID: ");
            }

            while (!activityChoices.ContainsKey(activityid))
            {
                Console.WriteLine("That activity does not exist! try again.");
                Console.Write("Select an activity ID: ");
                while (!Int32.TryParse(Console.ReadLine(), out activityid))
                {
                    Console.WriteLine("Invalid input. Please enter an integer [0-99]");
                    Console.Write("Select an activity ID: ");
                }
            }

            Console.Write("Enter the time that the activity starts (YYYY-MM-DD HH:MM:SS): ");
            DateTime starttime;
            while (!DateTime.TryParse(Console.ReadLine(), out starttime))
            {
                Console.WriteLine("Invalid input. Use the provided format.");
                Console.Write("Enter the time that the activity starts (YYYY-MM-DD HH:MM:SS)");
            }

            Console.Write("Enter the time that the activity ends (YYYY - MM - DD HH: MM:SS): ");
            DateTime endtime;
            while (!DateTime.TryParse(Console.ReadLine(), out endtime))
            {
                Console.WriteLine("Invalid input. Use the provided format.");
                Console.Write("Enter the time that the activity ends (YYYY-MM-DD HH:MM:SS)");
            }

            List<Coach>? coachList = Coach.GetAllCoaches();
            Dictionary<string, Coach> coachChoices = new Dictionary<string, Coach>();

            Console.WriteLine("Available coaches:");
            Console.WriteLine("======");
            if (coachList == null || coachList.Count == 0)
            {
                Console.WriteLine("There are no coaches registered in the system. Add some first!");
                Console.WriteLine("======");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            foreach (Coach coach in coachList)
            {
                coachChoices.Add(coach.Username, coach);

                Console.WriteLine(
                    $"""
                    Username:       {coach.Username}
                    Name:           {coach.Name}
                    Address:        {coach.Address}
                    Phonenumber:    {coach.PhoneNumber}
                    Age:            {coach.Age}
                    Gender:         {coach.Gender}
                    ======
                    """);
            }

            Console.Write("Enter a coach username: ");
            string? coachusername = Console.ReadLine();
            while (coachusername == null || coachusername.Length == 0)
            {
                Console.WriteLine("The coach username cannot be empty. Try again.");
                Console.Write("Enter a coach username: ");
                coachusername = Console.ReadLine();
            }

            while (!coachChoices.ContainsKey(coachusername))
            {
                Console.WriteLine("That username does not exist. Try again.");

                Console.Write("Enter a coach username: ");
                coachusername = Console.ReadLine();
                while (coachusername == null || coachusername.Length == 0)
                {
                    Console.WriteLine("The coach username cannot be empty. Try again.");
                    Console.Write("Enter a coach username: ");
                    coachusername = Console.ReadLine();
                }
            }

            Console.Clear();
            Console.WriteLine(menuHeader);

            Activity chosenactivity = activityChoices[activityid];
            Console.WriteLine(
                $"""
                Name:           {chosenactivity.Name}
                Description:    {chosenactivity.Description}
                Start Time:     {starttime}
                End Time:       {endtime}
                Coach:          {coachChoices[coachusername].Name}
                Location:       {location.Name}
                ======
                """);

            Console.Write("Are you sure you want to plan this activity? (Y/N):  ");
            string? confirmation = Console.ReadLine();

            while (confirmation == null || confirmation.Length == 0)
            {
                Console.WriteLine("Input cannot be empty. Try again.");
                Console.Write("Are you sure you want to plan this activity? (Y/N):  ");
                confirmation = Console.ReadLine();
            }

            while (!(confirmation.ToUpper() == "Y" || confirmation.ToUpper() == "N"))
            {
                Console.WriteLine("Invalid input. Enter 'Y' or 'N'");

                Console.Write("Are you sure you want to plan this activity? (Y/N):  ");
                confirmation = Console.ReadLine();
                while (confirmation == null || confirmation.Length == 0)
                {
                    Console.WriteLine("Input cannot be empty. Try again.");
                    Console.Write("Are you sure you want to plan this activity? (Y/N):  ");
                    confirmation = Console.ReadLine();
                }
            }

            if (confirmation.ToUpper() == "N")
            {
                Console.WriteLine("Planning activity CANCELLED");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            PlannedActivity? plannedActivity = PlannedActivity.CreatePlannedActivity(chosenactivity, starttime, endtime, coachChoices[coachusername], location.Id);

            if (plannedActivity == null)
            {
                Console.WriteLine("Something went wrong while uploading the PlannedActivity to the database.");
            } else
            {
                Console.WriteLine("Succesfully planned in the activity!");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
