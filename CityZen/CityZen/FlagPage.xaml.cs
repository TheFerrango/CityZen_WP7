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
        NetworkCoop nc;
        GeoCoordinateWatcher gcw;
        byte[] photoToSerial;
        bool hasNetwork;

        public FlagPage()
        {
            InitializeComponent();
            hasNetwork = Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable;
            photoToSerial = new byte[0];
            //Network Manager
            nc = new NetworkCoop();
            //nc.Wc.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(Wc_DownloadStringCompleted);

           

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

        void Wc_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            //ListPicker
            listPick.ItemsSource = Staticona.Dizione;
        }


        #endregion

        #region GPS Data management

        void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            mapLayer.Items.Clear();
            mapLayer.Items.Add(new Pushpin() { Location = e.Position.Location, Content = Languages.AppResources.FlagPosMap });
            mapBox.SetView(e.Position.Location, 14);
            (new OpenStreetMapsParser(txtCity, txtRoad)).GetStreetName(e.Position.Location);

        }

        void gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
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


        bool BigCheck()
        {
            btnSubmit.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            btnSubmit.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            if (CheckCity() && CheckRoad())
            {
                if (CheckDesc())
                {
                    btnSubmit.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                    btnSubmit.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                    return true;
                }

            }
            return false;

        }

        #endregion

        #region UI magic

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            remChars.Text = string.Format("{0}/{1}", txtbxDesc.Text.Length, txtbxDesc.MaxLength);
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mapBox.Focus();
            BigCheck();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();

            if (Staticona.Dizione == null)
            {
                if (hasNetwork)
                {
                    //nc.retrieveCategories();
                    Staticona.Riempilo();
                    listPick.ItemsSource = Staticona.Dizione;
                }
                else
                    MessageBox.Show(Languages.AppResources.MsgBxErrNoNet, Languages.AppResources.MsgBxErrTitle, MessageBoxButton.OK);
            }
            else
                listPick.ItemsSource = Staticona.Dizione;
        }

        #endregion

        #region Camera management code

        void cct_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                imgBox.Source = PictureDecoder.DecodeJpeg(e.ChosenPhoto, 640, 480);
                e.ChosenPhoto.Position = 0;
                List<byte> lb = new List<byte>();
                while (e.ChosenPhoto.Position < e.ChosenPhoto.Length)
                    lb.Add((byte)e.ChosenPhoto.ReadByte());
                photoToSerial = lb.ToArray();
                btnDelPh.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void btnTakePh_Click(object sender, RoutedEventArgs e)
        {
            cct.Show();
        }

        private void btnDelPh_Click(object sender, RoutedEventArgs e)
        {
            imgBox.Source = null;
            photoToSerial = new byte[0];

            btnDelPh.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //string bigPimp = string.Format("{0}, {1}, {2}, {3}: {4}", txtCity.Tag.ToString(), txtCity.Text, txtRoad.Text, ((ListPickerItem)listPick.SelectedItem).Content.ToString(), txtbxDesc.Text);
            if (BigCheck())
            {
                DataStructure toSend = new DataStructure()
                {
                    category = ((KeyValuePair<int, string>)listPick.SelectedItem).Key.ToString(),
                    country = txtCity.Tag.ToString(),
                    city = txtCity.Text,
                    address = txtRoad.Text,
                    description = txtbxDesc.Text,
                    image = Convert.ToBase64String(photoToSerial)
                };

                string c = Newtonsoft.Json.JsonConvert.SerializeObject(toSend);
                nc.sendData("data=" + c);
                NavigationService.Navigate(new Uri("/FlagPage.xaml?c=" + DateTime.Now.Millisecond, UriKind.Relative));
            }
            else
            {
                MessageBox.Show(Languages.AppResources.MsgBxErrVal, Languages.AppResources.MsgBxErrTitle, MessageBoxButton.OK);
            }
        }
    }
}