using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace CityZen
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (Staticona.Dizione == null)

                if (Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable)
                {
                    if( Staticona.Dizione == null)
                    {
                        NetworkCoop nc = new NetworkCoop();
                        nc.retrieveCategories();
                    }
                }
                else
                    MessageBox.Show(Languages.AppResources.MsgBxErrNoNet, Languages.AppResources.MsgBxErrTitle, MessageBoxButton.OK);
            
            //Staticona.Riempilo();
        }

        private void tileSubmit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable && Staticona.Dizione != null)
                NavigationService.Navigate(new Uri("/FlagPage.xaml", UriKind.Relative));
            else
                MessageBox.Show(Languages.AppResources.MsgBxErrNoNet, Languages.AppResources.MsgBxErrTitle, MessageBoxButton.OK);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
        }
    }
}