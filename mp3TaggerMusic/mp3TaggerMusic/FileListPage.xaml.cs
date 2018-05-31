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

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListPage : ContentPage
    {
        private HttpClient _client = new HttpClient();

        private string lastfmApiKey = "9b1e45575aa97afc4f34665f46148cc5";
        private string lastfmUrl = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key={0}&artist={1}&track={2}&format=json";
        private List<FilesData> getFlies(string path = "")
        {
            List<FilesData> fdl = new List<FilesData>();
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
                await DisplayAlert("Seleccionar Archivo", "Debe seleccionar un archivo", "Ok");
                return;
            }

           await Navigation.PushAsync(new EditSongPropPage(pickedFile));

        }
    }
}