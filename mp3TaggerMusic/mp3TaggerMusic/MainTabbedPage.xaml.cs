using mp3TaggerMusic.CustomCode;
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
        Page currentttt;

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
                   

            bool hasConnection = Utility.DeviceHasInternet();
            if (!hasConnection)
            {
                tbiAutoComplete.Icon = "autocomplete_ot_dis.png";
            }
            else
            {
                tbiAutoComplete.Icon = "autocomplete_ot.png";
            }

            this.CurrentPageChanged += (object sender, EventArgs e) => {
                var i = this.Children.IndexOf(this.CurrentPage);
                currentttt = this.CurrentPage;
                System.Diagnostics.Debug.WriteLine("Page No:" + i);
            };

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
                case "Select All":

                    try
                    {
                        if(currentttt is FileListPage)
                        {
                            var ss = ((FileListPage)currentttt);
                            ss._checkAllSongs = true;
                            ss.CheckAllSong();
                        }
                        //var mp = (MainTabbedPage)App.Current.MainPage;
                        //var current = mp.CurrentPage;

                        var mp = App.Current.MainPage;
                        //var currenta = mp.CurrentPage;

                        var x = Application.Current.MainPage.Navigation.NavigationStack.Last();
                        //var current = x.CurrentPage;


                    }
                    catch (Exception ex)
                    {
                    }

                    //page1
                    //var currentPage = (NavigationPage).CurrentPage;
                    //if (currentPage != null && currentPage is ContentPage)
                    //{
                    //    TopViewController.NavigationItem.RightBarButtonItems = new UIBarButtonItem[0];
                    //    return;
                    //}

                    await DisplayAlert("Informacion", "Seleccionadas Todas", "Ok");

                    break;
                case "Autocomplete Selecteds Songs":
                    bool hasConnection = Utility.DeviceHasInternet();
                    if (!hasConnection)
                    {
                        tbi.Icon = "autocomplete_ot_dis.png";

                        await DisplayAlert("No Conexion", "Debe estar conectado a internet para usar esta funcionalidad.", "Ok");
                        return;
                    }

                    tbi.Icon = "autocomplete_ot.png";

                    await DisplayAlert("Informacion", "autocompletadas las seleccionadas", "Ok");
                    break;
                default:
                    break;
            }
        }
    }
}