using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using mp3TaggerMusic.Models;
using Plugin.FilePicker;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using mp3TaggerMusic.CustomCode;
using PCLStorage;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListPage : ContentPage
    {        
        private async Task<List<FilesData>> getFilesOnPath(string path = "")
        {
            List<FilesData> fdl = new List<FilesData>();


            var folder = await FileSystem.Current.GetFolderFromPathAsync(path);
            var allfiles = folder.GetFilesAsync();

            var f = new FilesData() { NameFile = "Prueba", Extension = ".mp3", Path = "c/wndows/a" };
            fdl.Add(f);

            f = new FilesData() { NameFile = "Prueba 2", Extension = ".mp4", Path = "c/wndows/b" };
            fdl.Add(f);

            return fdl;
        }


        public FileListPage()
        {
            InitializeComponent();

            //lvFiles.ItemsSource = getFlies();            
        }

        protected override async void OnAppearing()
        {//test api
            try
            {/*
                string songTitle = "the theft";
                string songArtist = "atreyu";

                string realUrl = string.Format(lastfmUrl, lastfmApiKey, System.Net.WebUtility.UrlEncode(songArtist), System.Net.WebUtility.UrlEncode(songTitle));
                
                var content = await _client.GetStringAsync(realUrl);
             
                //var songData = JsonConvert.DeserializeObject<SongInfoClass.SongObject>(content);   //.Replace("#text","text").Replace("@attr", "attr")
                

                //var posts = JsonConvert.DeserializeObject<List<Post>>(content);

                //_posts = new ObservableCollection<Post>(posts);
                //postsListView.ItemsSource = _posts;
                */

            }
            catch (Exception ex)
            {
                throw;
            }
            base.OnAppearing();
        }

        private async void btnFileSel_Clicked(object sender, EventArgs e)
        {
            var pickedFile = await CrossFilePicker.Current.PickFile();

            if (pickedFile == null)
            {
                await DisplayAlert("Archivo Audio", "Debe seleccionar un archivo de audio", "Ok");
                return;
            }

            Utility.Show();

            await Navigation.PushAsync(new EditSongPropPage(pickedFile));

            Utility.Hide();
        }

        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            ToolbarItem tbi = (ToolbarItem)sender;
            if (tbi.Text == "Settings")
            {
                var page = new SettingsPage();
                await Navigation.PushAsync(page);
            }
        }

        private async void btnFileSelPath_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Informacion", "Debe seleccionar un archivo de audio para asi obtener todos los archivos de audio compatibles de dicha ubicacion", "Ok");

            var pickedFile = await CrossFilePicker.Current.PickFile();

            if (pickedFile == null)
            {
                await DisplayAlert("Archivo Audio", "Debe seleccionar un archivo de audio", "Ok");
                return;
            }

            Utility.Show();

            await getFilesOnPath(pickedFile.FilePath);

            Utility.Hide();
        }
    }
}