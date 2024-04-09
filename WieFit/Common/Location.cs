using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

internal class Location
{
    private readonly DAL database = DAL.Instance;
    public int Id { get; set; }
    public string Name { get; set; }
    public string Adress { get; set; }
    public string Postalcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }


    public Location(int _id, string _name, string _adress, string _postalcode, string _city, string _country)
    {
        Id = _id;
        Name = _name;
        Adress = _adress;
        Postalcode = _postalcode;
        City = _city;
        Country = _country;
    }

    public Location(string _name, string _adress, string _postalcode, string _city, string _country)
    {
        Name = _name;
        Adress = _adress;
        Postalcode = _postalcode;
        City = _city;
        Country = _country;
    }

    public bool AddLocation()
    {
        return database.Addlocation(this);
    }
    public bool DeleteLocation()
    {
        return database.DeleteLocation(this);
    }


}
