using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
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
            
            CurrentPage = null;
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