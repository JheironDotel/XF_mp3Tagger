﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using mp3TaggerMusic.Models;
using Plugin.FilePicker;
using mp3TaggerMusic.CustomCode;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileListPage : ContentPage
    {
        private bool wasLoad = false;
        private bool wasChecked = false;
        private List<SongFilesData> allData;

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
                            Selected = isChecked,
                            SongNameLower = infoSong.SongName.ToLower()
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

            CheckStoragePermissions();

            Utility.Show();

            if (!wasLoad)
            {
                allData = await getAllSongFiles();
                lvTracksFiles.ItemsSource = allData;
            }
            else if (App.Global_refreshListSong)
            {
                allData = await getAllSongFiles();

                lvTracksFiles.ItemsSource = allData;//await getAllSongFiles();
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
                if (songfilesdata == null)
                {
                    if (e.Item != null)
                    {
                        //songfilesdata = e.Item;
                    }
                }
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

        private async void sbTracks_TextChanged(object sender, TextChangedEventArgs e)
        {
            //lvTracksFiles.BeginRefresh();
            //thats all you need to make a search  
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                lvTracksFiles.ItemsSource = await getAllSongFiles();
            }
            else            
            {
                var search = from a in allData
                             where a.SongNameLower.Contains(e.NewTextValue.ToLower())
                             select //a;
                             new
                             {
                                 SongFilePath= a.SongFilePath,
                                 SongName=a.SongName,
                                 ArtistName = a.ArtistName,
                                 Selected = a.Selected,
                                 //a.AlbumCover
                             };


                lvTracksFiles.ItemsSource = search; //allData.Where(x => x.SongNameLower.StartsWith(e.NewTextValue.ToLower()));
                //lvTracksFiles.EndRefresh();
            }
        }
    }
}