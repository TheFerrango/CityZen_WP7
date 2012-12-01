using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Device.Location;

namespace CityZen
{
    public class OpenStreetMapsParser
    {
        WebClient wc;
        TextBox city, road;

        public OpenStreetMapsParser(TextBox _ct, TextBox _rd)
        {
            wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            city = _ct;
            road = _rd;
        }

        public void GetStreetName(GeoCoordinate gc)
        {
            wc.DownloadStringAsync(new Uri(string.Format("http://nominatim.openstreetmap.org/reverse?format=json&lat={0}&lon={1}&zoom=18&addressdetails=1", gc.Latitude, gc.Longitude)));
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            AddressClass cc =  JsonConvert.DeserializeObject<AddressClass>(e.Result);
            city.Tag = cc.address["country"];
            city.Text = cc.address["city"];
            road.Text = cc.address["road"];
        }

    }
}
