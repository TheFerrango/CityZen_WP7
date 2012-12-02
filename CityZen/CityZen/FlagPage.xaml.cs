using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Tasks;

namespace CityZen
{
    public partial class FlagPage : PhoneApplicationPage
    {
        #region Constructor & variables

        PhotoChooserTask cct;
        GeoCoordinateWatcher gcw;
        WriteableBitmap wrbmp;
        byte[] photoToSerial;

        public FlagPage()
        {
            InitializeComponent();

            //Camera
            cct = new PhotoChooserTask();
            cct.ShowCamera = true;
            cct.PixelHeight = 480;
            cct.PixelWidth = 640;
            cct.Completed += new EventHandler<PhotoResult>(cct_Completed);

            //GPS
            gcw = new GeoCoordinateWatcher();
            gcw.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(gcw_StatusChanged);            
            gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
            gcw.Start();            
        }


        #endregion

        #region GPS Data management

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

        #endregion

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

        #region UI magic

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            remChars.Text = string.Format("{0}/{1}", txtbxDesc.Text.Length, txtbxDesc.MaxLength);
        }
        
        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BigCheck();
        }

        #endregion

        #region Camera management code

        void cct_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                wrbmp = PictureDecoder.DecodeJpeg(e.ChosenPhoto, 640, 480);
                e.ChosenPhoto.Position = 0;
                List<byte> lb = new List<byte>();
                while(e.ChosenPhoto.Position < e.ChosenPhoto.Length)
                    lb.Add((byte)e.ChosenPhoto.ReadByte());
                photoToSerial = lb.ToArray();
                imgBox.Source = wrbmp;
            }
        }

        private void btnTakePh_Click(object sender, RoutedEventArgs e)
        {
            cct.Show();
        }

        private void btnDelPh_Click(object sender, RoutedEventArgs e)
        {
            wrbmp = null;
            photoToSerial = new byte[0];
        }

        #endregion

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //string bigPimp = string.Format("{0}, {1}, {2}, {3}: {4}", txtCity.Tag.ToString(), txtCity.Text, txtRoad.Text, ((ListPickerItem)listPick.SelectedItem).Content.ToString(), txtbxDesc.Text);
            DataStructure toSend = new DataStructure()
            {
                category = ((ListPickerItem)listPick.SelectedItem).Content.ToString(),
                country = txtCity.Tag.ToString(),
                city = txtCity.Text,
                address = txtRoad.Text,
                description = txtbxDesc.Text,
                image = Convert.ToBase64String(photoToSerial)
            };

            string c = Newtonsoft.Json.JsonConvert.SerializeObject(toSend);
            NetworkCoop nc = new NetworkCoop();
            nc.sendData("data="+c);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}