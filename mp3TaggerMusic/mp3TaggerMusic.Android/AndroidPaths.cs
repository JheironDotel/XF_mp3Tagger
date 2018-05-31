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
using System.Runtime.CompilerServices;

[assembly: Xamarin.Forms.Dependency(typeof(mp3TaggerMusic.Intefaces.IPathService))]
namespace mp3TaggerMusic.Droid
{
    public class AndroidPaths : Intefaces.IPathService
    {
        public string InternalFolder
        {
            get
            {
                return Android.App.Application.Context.FilesDir.AbsolutePath;
            }
        }

        public string PublicExternalFolder
        {
            get
            {
                return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            }
        }

        public string PrivateExternalFolder
        {
            get
            {
                return Application.Context.GetExternalFilesDir(null).AbsolutePath;
            }
        }
    }
}