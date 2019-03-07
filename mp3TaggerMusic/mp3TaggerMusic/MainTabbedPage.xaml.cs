using mp3TaggerMusic.CustomCode;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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

            CheckStoragePermissions();

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

        public async void CheckStoragePermissions()
        {
            string message = "";
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        message = "Se necesita poder leer y escribir en los archivos para ubicar los archivos de audio compatibles y poder modificarles las informaciones";
                        await DisplayAlert("Lectura y Escritura", message, "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Storage))
                        status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    //aqui por si quiero hacer alguna llamada en especficico
                    //var results = await mi metodo/funcion                    
                }
                else if (status != PermissionStatus.Unknown)
                {
                    message = "Estos permisos son importantes para el funciomiento de la aplicacion, se cerrara la aplicacion";
                    await DisplayAlert("Lectura y Escritura Denegada", message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}