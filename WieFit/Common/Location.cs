using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

internal class Location
{
    public int id { get; set; }
    public string name { get; set; }
    public string adress { get; set; }
    public string postalcode { get; set; }
    public string city { get; set; }
    public string country { get; set; }


    public Location(int _id, string _name, string _adress, string _postalcode, string _city, string _country)
    {
        id = _id;
        name = _name;
        adress = _adress;
        postalcode = _postalcode;
        city = _city;
        country = _country;
    }

    public Location(string _name, string _adress, string _postalcode, string _city, string _country)
    {
        name = _name;
        adress = _adress;
        postalcode = _postalcode;
        city = _city;
        country = _country;
    }

    public static void AddLocation()
    {

        Console.Write("Enter location name: ");
        string _locationname = (Console.ReadLine());

        Console.Write("Enter location adress: ");
        string _locationadress = Console.ReadLine();

        Console.Write("Enter postalcode: ");
        string _postalcode = Console.ReadLine();

        Console.Write("Enter city: ");
        string _city = Console.ReadLine();

        Console.Write("Enter country: ");
        string _country = Console.ReadLine();
        Location location = new Location(
            _locationname,
            _locationadress,
            _postalcode,
            _city,
            _country
            );

    }


}
