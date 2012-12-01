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
using System.Windows.Media.Imaging;

namespace CityZen
{
    public class DataStructure
    {
        string _country, _city, _address, _category, _description;
        WriteableBitmap _image;

        public string country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        public string address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

        public WriteableBitmap image
        {
            get { return _image; }
            set { _image = value; }
        }
    }
}
