using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common;
using WieFit.Common.DAL;

internal class Location
{
    private readonly LocationDAL locationDAL = LocationDAL.Instance;
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Postalcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public Planning Planning { get; set; }


    public Location(int _id, string _name, string address, string _postalcode, string _city, string _country, Planning planning)
    {
        Id = _id;
        Name = _name;
        Address = address;
        Postalcode = _postalcode;
        City = _city;
        Country = _country;
        Planning = planning;
    }

    public Location(string _name, string address, string _postalcode, string _city, string _country)
    {
        Name = _name;
        Address = address;
        Postalcode = _postalcode;
        City = _city;
        Country = _country;
        Planning = new Planning();
    }

    public bool AddLocation()
    {
        if (!Planning.CreatePlanning()) return false;
        return locationDAL.Addlocation(this);
    }
    public bool DeleteLocation()
    {
        return locationDAL.DeleteLocation(this);
    }


}
