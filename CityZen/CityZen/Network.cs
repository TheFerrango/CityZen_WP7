using System;
using System.Net;
using Newtonsoft.Json;
using Coding4Fun.Phone.Controls;
using System.Collections.Generic;

namespace CityZen
{
    public class NetworkCoop
    {
        string SERVER_ADDRESS = "http://192.168.29.196:8000/";

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
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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
                Staticona.Dizione = JsonConvert.DeserializeObject<Dictionary<int, string>>(e.Result);
                tp.Title = Languages.AppResources.MsgBxAllGTitle;
                tp.Message = Languages.AppResources.MsgBxAllGBody;
            }
            tp.Show();
            netOperation = false;
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
            netOperation = false;
        }

        public void sendData(string bah)
        {
            netOperation = true;
            

            wc.UploadStringAsync(new Uri(SERVER_ADDRESS + "api/"), bah);
            
        }

        public void retrieveCategories()
        {
            netOperation = true;
            wc.DownloadStringAsync(new Uri(SERVER_ADDRESS + "api/categories/"));
        }
    }
}
