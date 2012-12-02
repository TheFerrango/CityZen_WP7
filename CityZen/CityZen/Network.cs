using System;
using System.Net;
using Coding4Fun.Phone.Controls;

namespace CityZen
{
    public class NetworkCoop
    {
        WebClient wc;
        bool netOperation;

        public bool NetOperation
        {
            get { return netOperation; }
            set { netOperation = value; }
        }

        public NetworkCoop()
        {
            netOperation = false;
            wc = new WebClient();
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            ToastPrompt tp = new ToastPrompt();
            if (e.Error != null)
            {
                //MessageBox.Show(Languages.AppResources.MsgBxErrGen, Languages.AppResources.MsgBxErrTitle, MessageBoxButton.OK);
                tp.Title = Languages.AppResources.MsgBxErrTitle;
                tp.Message = Languages.AppResources.MsgBxErrGen;
            }
            else
            {
                //MessageBox.Show(Languages.AppResources.MsgBxAllGBody, Languages.AppResources.MsgBxAllGTitle, MessageBoxButton.OK);
                tp.Title = Languages.AppResources.MsgBxAllGTitle;
                tp.Message = Languages.AppResources.MsgBxAllGBody;
            }
            tp.Show();
        }

        public void sendData(string bah)
        {
            netOperation = true;
            

            wc.UploadStringAsync(new Uri("http://192.168.1.10:8000/api/"), bah);
            
        }
    }
}
