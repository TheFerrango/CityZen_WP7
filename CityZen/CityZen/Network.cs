using System;
using System.Net;
using Newtonsoft.Json;
using Coding4Fun.Phone.Controls;
using System.Collections.Generic;
using Microsoft.Phone.Controls;


namespace CityZen
{
    public class NetworkCoop
    {
        //string SERVER_ADDRESS = "http://192.168.29.196:8000/";
        string SERVER_ADDRESS = "http://192.168.28.184:8000/";
        //string SERVER_ADDRESS = "http://10.23.4.184:8000/";

        WebClient wc;

        public WebClient Wc
        {
            get { return wc; }
            set { wc = value; }
        }

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

        public bool sendData(string bah)
        {
            if (!netOperation)
            {
                netOperation = true;
                wc.UploadStringAsync(new Uri(SERVER_ADDRESS + "api/"), bah);
            }
            else
                return false;
            return true;
        }

        public bool retrieveCategories()
        {
            if (!netOperation)
            {
                netOperation = true;
                wc.DownloadStringAsync(new Uri(SERVER_ADDRESS + "api/categories/"));
            }
            else
                return false;
            return true;
        }
    }
}
