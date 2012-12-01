using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace CityZen
{
    public class AddressClass
    {
        Dictionary<string, string> _fuck;

        public Dictionary<string, string> address
        {
            get { return _fuck; }
            set { _fuck = value; }
        }
/*
        public string road
        {
            get { return _road; }
            set { _road = value; }
        }
        string _city;

        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
 */
    }
}
