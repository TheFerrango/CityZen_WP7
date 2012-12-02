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
        }

        private void tileSubmit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FlagPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
        }
    }
}