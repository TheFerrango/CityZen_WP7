using System;
using System.Device.Location;
using System.Net;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Windows;

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
            if (e.Error == null)
            {
                try
                {
                    AddressClass cc = JsonConvert.DeserializeObject<AddressClass>(e.Result);
                    city.Tag = cc.address["country"];
                    city.Text = cc.address["city"];
                    road.Text = cc.address["road"];
                }
                catch
                {
                }
            }
        }

    }
}
