
using System;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        //Page currentttt;
        private bool wasLoad = false;

        public MainTabbedPage()
        {
            InitializeComponent();

            //example NavigationPage
            //var navigationPage = new NavigationPage(new FileListPage());
            //navigationPage.Icon = "";
            //navigationPage.Title = "All Songs";
            //Children.Add(navigationPage);

            Children.Add(new FileListPage() { Title = "All Tracks", Icon = "" });
            Children.Add(new MainPage() { Title = "Track", Icon = "" });

            //CurrentPage = null;

            //this.CurrentPageChanged += (object sender, EventArgs e) => {
            //    var i = this.Children.IndexOf(this.CurrentPage);
            //    currentttt = this.CurrentPage;
            //    System.Diagnostics.Debug.WriteLine("Page No:" + i);
            //};
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var currPage = CurrentPage;

            if (currPage is FileListPage)
            {
                if (!wasLoad || App.Global_refreshListSong)
                {
                    //((FileListPage)currPage)

                    var x = CurrentPage as FileListPage;
                    //x.lvTracksFiles.ItemSource = 0;               

                    /*currPage.lvTracksFiles.ItemsSource = await getAllSongFiles();
                    App.Global_refreshListSong = false;*/
                }
            }


        }


        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            ToolbarItem tbi = (ToolbarItem)sender;
            switch (tbi.Text)
            {
                case "Settings":
                    var page = new SettingsPage();
                    await Navigation.PushAsync(page);
                    break;
                default:
                    break;
            }
        }

        
    }
}