using mp3TaggerMusic.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace mp3TaggerMusic
{
    public partial class App : Application
    {
        private string OnlyCompleteMissingInfoKey = "true";

        public bool OnlyCompleteMissingInfo
        {
            get
            {
                if (Properties.ContainsKey(OnlyCompleteMissingInfoKey))
                {
                    return Convert.ToBoolean(Properties[OnlyCompleteMissingInfoKey]);
                }
                return true;
            }
            set { Properties[OnlyCompleteMissingInfoKey] = value; }
        }


        public static bool Global_refreshListSong { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IProgressInterface>();
            DependencyService.Register<IPathService>();
            DependencyService.Register<IFileList>();
            DependencyService.Register<IConnectivityChecker>();

            MainPage = new NavigationPage(new FileListPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }


}
