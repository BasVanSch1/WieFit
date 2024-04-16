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
    private static readonly LocationDAL locationDAL = LocationDAL.Instance;
    private static readonly ActivityDAL activityDAL = ActivityDAL.Instance;

    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Postalcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public List<PlannedActivity>? PlannedActivities { get; private set; }


    public Location(int _id, string _name, string address, string _postalcode, string _city, string _country)
    {
        Id = _id;
        Name = _name;
        Address = address;
        Postalcode = _postalcode;
        City = _city;
        Country = _country;
        PlannedActivities = GetPlannedActivities();
    }

    public Location(string _name, string address, string _postalcode, string _city, string _country)
    {
        Name = _name;
        Address = address;
        Postalcode = _postalcode;
        City = _city;
        Country = _country;
    }

    public bool AddLocation()
    {
        return locationDAL.Addlocation(this);
    }
    public bool DeleteLocation()
    {
        return locationDAL.DeleteLocation(this);
    }

    public static List<Location>? GetAllLocations()
    {
        return locationDAL.GetAllLocations();
    }

    public static Location? GetLocation(int id)
    {
        return locationDAL.GetLocation(id);
    } 

    private List<PlannedActivity>? GetPlannedActivities()
    {
        return activityDAL.GetPlannedActivities(this);
    }
}
