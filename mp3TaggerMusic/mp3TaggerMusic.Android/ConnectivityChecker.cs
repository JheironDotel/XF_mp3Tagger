using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using mp3TaggerMusic.Intefaces;
using mp3TaggerMusic.Droid;
using Xamarin.Forms;
using Android.Net;

[assembly: Dependency(typeof(ConnectivityChecker))]
namespace mp3TaggerMusic.Droid
{
    public class ConnectivityChecker : IConnectivityChecker
    {
        private ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
        private NetworkInfo networkInfo;

        public ConnectivityChecker() { }

        /// <summary>
        /// Independientemente de la conectividad (revisa si tiene Wifi, Data o Roaming)
        /// </summary>
        /// <returns></returns>
        public bool DeviceHasInternet()
        {
            networkInfo = connectivityManager.ActiveNetworkInfo;
            if (networkInfo == null)
            {
                return false;
            }

            bool isOnline = networkInfo.IsConnected;

            return isOnline;
        }

        /// <summary>
        /// Revisa si el dispositivo tiene Conexion Roaming
        /// </summary>
        /// <returns></returns>
        public bool DeviceHasRoaming()
        {
            if (networkInfo == null)
            {
                return false;
            }
            return networkInfo.IsRoaming;
        }

        /// <summary>
        /// Revisa si el dispositivo tiene Conexion Wifi
        /// </summary>
        /// <returns></returns>
        public bool DeviceHasWifi()
        {
            if (networkInfo == null)
            {
                return false;
            }
            return networkInfo.Type == ConnectivityType.Wifi; ;
        }
    }
}