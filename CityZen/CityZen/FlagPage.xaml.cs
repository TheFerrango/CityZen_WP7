using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;

namespace CityZen
{
    public partial class FlagPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher gcw;

        public FlagPage()
        {
            InitializeComponent();
            gcw = new GeoCoordinateWatcher();
            gcw.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(gcw_StatusChanged);
            
            gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
            gcw.Start();            
        }

        void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            mapLayer.Items.Clear();
            mapLayer.Items.Add(new Pushpin() { Location = e.Position.Location, Content= Languages.AppResources.FlagPosMap });
            mapBox.SetView(e.Position.Location, 14);
            (new NetworkOperation()).GetStreetName(e.Position.Location);

        }

        void gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch(e.Status)
            {
                case GeoPositionStatus.Ready:
                    status.Visibility = System.Windows.Visibility.Collapsed;
                    mapBox.Visibility = System.Windows.Visibility.Visible;
                    
                    break;
                case GeoPositionStatus.Disabled:
                    MessageBox.Show(Languages.AppResources.MsgBxErrTitle, Languages.AppResources.MsgBxErrBody, MessageBoxButton.OK);
                    break;
            }   
        }
    }
}