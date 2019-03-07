using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using mp3TaggerMusic.Models;
using Plugin.FilePicker;
using mp3TaggerMusic.CustomCode;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListPage : ContentPage
    {
        private bool wasLoad = false;
        private bool wasChecked = false;

        public FileListPage()
        {
            InitializeComponent();

            bool hasConnection = Utility.DeviceHasInternet();
            if (!hasConnection)
            {
                btnAutoComplete.Image = "autocomplete_ot_dis.png";
            }
            else
            {
                btnAutoComplete.Image = "autocomplete_o.png";
            }
        }

        private async Task<List<SongFilesData>> getAllSongFiles(bool isChecked = false)
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
                            SongFilePath = infoSong.SongFilePath,
                            Selected = isChecked
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

            //AQUI RECIBO LA VARIABLE QUE SETIE Y SI ES TRUE SETEO "wasLoad" COMO FALSO PARA QUE
            //SE REFRESQUE EL LISTVIEWE, SINO LLAMO AL METODO REFRESH Y YA

            //if (App.refreshListSong)
            //{
            //    lvTracksFiles.Refreshing += lvTracksFiles_Refreshing;
            //    App.refreshListSong = false;
            //}

            if (!wasLoad || App.Global_refreshListSong)
            {
                lvTracksFiles.ItemsSource = await getAllSongFiles();
                App.Global_refreshListSong = false;
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
            try
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
            catch (Exception ex)
            {

                throw;
            }
        }

        private async void btnCheckAll_Clicked(object sender, EventArgs e)
        {
            if (!wasChecked)
            { wasChecked = true; }
            else { wasChecked = false; }

            lvTracksFiles.ItemsSource = await getAllSongFiles(wasChecked);
            lvTracksFiles.EndRefresh();
        }

        private async void btnAutoComplete_Clicked(object sender, EventArgs e)
        {
            var allSfd = lvTracksFiles.ItemsSource as List<SongFilesData>;
            var allcheckeds = allSfd.Where(x => x.Selected).ToList();
            if (allcheckeds.Count() > 0)
            {
                foreach (SongFilesData sfd in allcheckeds)
                {
                    //mi proceso de actualizar
                    await DisplayAlert("Informacion", "La cancion: " + sfd.SongName + " fue seleccionada", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Advertencia", "Debe seleccionar al menos una cancion.", "Ok");
            }
        }
              
                
    }
}