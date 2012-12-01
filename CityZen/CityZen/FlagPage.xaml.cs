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
        string category;

        public FlagPage()
        {
            InitializeComponent();
            category = "";
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
            (new OpenStreetMapsParser(txtCity, txtRoad)).GetStreetName(e.Position.Location);

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            remChars.Text = string.Format("{0}/{1}", txtbxDesc.Text.Length, txtbxDesc.MaxLength);
        }

        #region CheckBeforeSend

        bool CheckCity()
        {
            return txtCity.Text != "";
        }

        bool CheckRoad()
        {
            return txtRoad.Text != "";
        }

        bool CheckDesc()
        {
            return txtbxDesc.Text != "";
        }

        bool CheckCheck()
        {
            //return category != "";
            return true;
        }

        void BigCheck()
        {
            btnSubmit.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            if (CheckCity() && CheckRoad())
            {
                btnSubmit.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

                if (CheckCheck() && CheckDesc()) 
                {
                    btnSubmit.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                } 

            }       
         
        }

        #endregion

              private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string bigPimp = string.Format("{0}, {1}, {2}, {3}: {4}", txtCity.Tag.ToString(), txtCity.Text, txtRoad.Text, ((ListPickerItem)listPick.SelectedItem).Content.ToString(), txtbxDesc.Text);
            MessageBox.Show(bigPimp);
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BigCheck();
        }
    }
}