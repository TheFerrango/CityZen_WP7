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
    public static class Staticona
    {
        static Dictionary<int, string> _dizione;

        public static Dictionary<int, string> Dizione
        {
            get { return Staticona._dizione; }
            set { Staticona._dizione = value; }
        }

        public static void Riempilo()
        {
            _dizione = new Dictionary<int,string>();
            _dizione.Add(0, "Architectural barriers");
            _dizione.Add(1, "Vandalism & Incivilities");
        }

    }
}
