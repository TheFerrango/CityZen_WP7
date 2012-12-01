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

namespace CityZen
{
    public class NetworkCoop
    {
        WebClient wc;

        public NetworkCoop()
        {
            wc = new WebClient();
         }

        public void sendData(string bah)
        {
            wc.UploadStringAsync(new Uri("http://192.168.29.196:8000/api/"), bah);
        }
    }
}
