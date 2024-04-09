﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WieFit.Common.DAL;

internal class Location
{
    private readonly DAL database = DAL.Instance;
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

    public bool AddLocation()
    {
        return database.Addlocation(this);
    }
    public bool DeleteLocation()
    {
        return database.DeleteLocation(this);
    }


}
