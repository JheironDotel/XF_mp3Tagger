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
        private bool wasLoad = false;

        public FileListPage()
        {
            InitializeComponent();
        }

        private async Task<List<SongFilesData>> getAllSongFiles()
        {
            List<SongFilesData> sfd = new List<SongFilesData>();
            try
            {
                var songsPaths = DependencyService.Get<Intefaces.IFileList>().GetAllSongFiles(Utility.AudioExtensionList);

                foreach (var path in songsPaths)
                {
                    var infoSong = await Utility.BasicInfoSong(path);
                    if (infoSong != null)
                    {
                        sfd.Add(new SongFilesData()
                        {
                            SongName = infoSong.SongName,
                            ArtistName = infoSong.ArtistName,
                            AlbumName = infoSong.AlbumName,
                            AlbumCover = infoSong.AlbumCover,
                            SongFilePath = infoSong.SongFilePath
                        });
                    }
                }
                wasLoad = true;
            }
            catch (Exception ex)
            {
                Utility.Hide();
            }
            return sfd;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Utility.Show();

            if (!wasLoad)
            {
                lvTracksFiles.ItemsSource = await getAllSongFiles();
            }

            Utility.Hide();

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

            try
            {
                var aaa = DependencyService.Get<Intefaces.IFileList>().GetAllSongFiles(new string[] { });

                var z = DependencyService.Get<Intefaces.IPathService>().InternalFolder;
                var w = DependencyService.Get<Intefaces.IPathService>().PrivateExternalFolder;
                var wa = DependencyService.Get<Intefaces.IPathService>().PublicExternalFolder;





            }
            catch (Exception ex)
            {

                throw;
            }


            //await getFilesOnPath(pickedFile.FilePath);

            Utility.Hide();
        }

        private async void lvTracksFiles_Refreshing(object sender, EventArgs e)
        {            
            lvTracksFiles.ItemsSource = await getAllSongFiles();
            lvTracksFiles.EndRefresh();
        }

        private async void lvTracksFiles_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Utility.Show();

            var songfilesdata = e.Item as SongFilesData;
            if (songfilesdata != null)
            {
                await Navigation.PushAsync(new EditSongPropPage(null, songfilesdata.SongFilePath));
            }
            else
            {
                await DisplayAlert("Advertencia", "Debe seleccionar una cancion", "Ok");
            }
            ((ListView)sender).SelectedItem = null;

            Utility.Hide();
        }
    }
}