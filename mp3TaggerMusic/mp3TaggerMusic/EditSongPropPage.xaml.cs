﻿using PCLStorage;

using Plugin.FilePicker.Abstractions;
using mp3TaggerMusic.CustomCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using mp3TaggerMusic.Models;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSongPropPage : ContentPage
    {
        private FileData _pickedFile;
        private int tapCount = 0;
        private bool changedCover = false;
        private byte[] changedCoverImg;

        private Stream streamR;
        private string picketFileName;
        private string picketFilePath;

        public EditSongPropPage(FileData pickedFile = null)
        {
            InitializeComponent();

            _pickedFile = pickedFile;
            FillSongInfo(pickedFile);

            #region Revisando si el dispositivo tiene acceso a internet para poder usar la caracteristica de autocompletar
            bool hasConnection = DependencyService.Get<Intefaces.IConnectivityChecker>().DeviceHasInternet();

            if (!hasConnection)
            {
                //btnAutoComplete.IsEnabled = false;
                btnAutoComplete.Image = "autocomplete_dis.png";
            }
            else
            {
                //btnAutoComplete.IsEnabled = true;
                btnAutoComplete.Image = "autocomplete.png";
            }
            #endregion
        }

        public async void FillSongInfo(FileData pickedFile)
        {
            try
            {
                picketFileName = pickedFile.FileName;
                picketFilePath = pickedFile.FilePath;

                var fExtension = Path.GetExtension(picketFileName);
                if (!Utility.AudioExtensionList.Contains(fExtension))
                {
                    string msj = string.Format("Los tipos de archivos de audio aceptados son: {0}", string.Join(", ", Utility.AudioExtensionList));
                    await DisplayAlert("Archivo Audio", msj, "Ok");
                    return;
                }


                byte[] fileData = pickedFile.DataArray;

                streamR = new MemoryStream();

                var folder = await FileSystem.Current.GetFileFromPathAsync(picketFilePath);

                using (streamR = await folder.OpenAsync(FileAccess.ReadAndWrite))
                {
                    TagLib.Id3v2.Tag.DefaultVersion = 3; TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                    var tagFile = //TagLib.File.Create(new TagLib.StreamFileAbstraction(name, streamR, streamW));
                        TagLib.File.Create(new FileAbstraction(picketFileName, streamR));
                    var tags = tagFile.GetTag(TagLib.TagTypes.Id3v2);

                    if (tags != null)
                    {
                        var realArtist = tags.AlbumArtists.Count() > 0 ? tags.AlbumArtists : tags.Performers;

                        txtSongTitle.Text = !string.IsNullOrEmpty(tags.Title) ? tags.Title : txtSongTitle.Text;
                        txtArtist.Text = string.Join(", ", realArtist);
                        txtAlbum.Text = !string.IsNullOrEmpty(tags.Album) ? tags.Album : txtAlbum.Text;
                        txtTrackNo.Text = !string.IsNullOrEmpty(tags.Track.ToString()) ? tags.Track.ToString() : txtTrackNo.Text;
                        txtYear.Text = !string.IsNullOrEmpty(tags.Year.ToString()) ? tags.Year.ToString() : txtYear.Text;
                        txtGenre.Text = string.Join(", ", tags.Genres);

                        if (tags.Pictures.Count() > 0)
                        {
                            var cover = tags.Pictures.FirstOrDefault().Data.Data;
                            Stream coverStream = new MemoryStream(cover);

                            imgCoverArt.Source = ImageSource.FromStream(() => coverStream);
                            coverStream.Dispose();
                        }
                        /*else
                        {
                            imgCoverArt.Source = ImageSource.FromResource("coverart.png");
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                streamR.Dispose();

                string msj = string.Format("Ha ocurrido un error: {0}", ex);
                await DisplayAlert("Error", msj, "Ok");

            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                byte[] fileData = _pickedFile.DataArray;

                var folder = await FileSystem.Current.GetFileFromPathAsync(picketFilePath);

                using (streamR = await folder.OpenAsync(FileAccess.ReadAndWrite))
                {
                    TagLib.Id3v2.Tag.DefaultVersion = 3; TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                    using (var tagFile = TagLib.File.Create(new FileAbstraction(picketFileName, streamR)/*TagLib.File.Create(new TagLib.StreamFileAbstraction(name, streamR, streamW)*/))
                    {
                        var tags = tagFile.Tag;

                        string[] realArtist = new string[] { };
                        string[] realGenres = new string[] { };
                        if (!string.IsNullOrEmpty(txtArtist.Text))
                        {
                            var sp = txtArtist.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            if (sp.Count() > 0)
                            {
                                realArtist = sp;
                            }
                        }

                        if (!string.IsNullOrEmpty(txtGenre.Text))
                        {
                            var sp = txtGenre.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            if (sp.Count() > 0)
                            {
                                realGenres = sp;
                            }
                        }


                        tags.Title = txtSongTitle.Text.FullTrimText();
                        tags.Performers = realArtist;
                        tags.AlbumArtists = realArtist;
                        tags.Album = txtAlbum.Text.FullTrimText();
                        tags.Track = txtTrackNo.Text.ToUInt();
                        tags.Year = txtYear.Text.ToUInt();
                        tags.Genres = realGenres;

                        if (changedCover)
                        {
                            Stream coverStream = new MemoryStream(changedCoverImg);
                            var tagCover = new TagLib.StreamFileAbstraction("hola", coverStream, coverStream);
                            tagFile.Tag.Pictures = new[] { new TagLib.Picture(tagCover) };
                            coverStream.Dispose();
                        }
                        tagFile.Save();
                        tagFile.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                streamR.Dispose();
                string msj = string.Format("Ha ocurrido un error: {0}", ex);
                await DisplayAlert("Error", msj, "Ok");
            }
        }

        async void OnDobleTapSearchImage(object sender, EventArgs args)
        {
            try
            {
                tapCount++;
                var imageSender = (Image)sender;
                if (tapCount % 2 == 0)
                {
                    var pickedFileImg = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();

                    if (pickedFileImg == null)
                    {
                        await DisplayAlert("Archivo Imagen", "Debe seleccionar un archivo de imagen", "Ok");
                        tapCount = 0;
                        return;
                    }

                    var fExtension = Path.GetExtension(pickedFileImg.FileName);
                    if (Utility.imgExtensionList.Contains(fExtension))
                    {
                        byte[] fileData = pickedFileImg.DataArray;
                        Stream coverStream = new MemoryStream(fileData);

                        imageSender.Source = ImageSource.FromStream(() => coverStream);
                        changedCover = true;
                        changedCoverImg = fileData;
                        tapCount = 0;
                    }
                    else
                    {
                        string msj = string.Format("Los tipos de imagenes aceptadas son: {0}", string.Join(", ", Utility.imgExtensionList));
                        await DisplayAlert("Archivo Imagen", msj, "Ok");
                        tapCount = 0;
                        return;
                    }
                }
            }
            catch (Exception)
            {
                tapCount = 0;
                throw;
            }
        }

        private async void btnAutoComplete_Clicked(object sender, EventArgs e)
        {
            try
            {
                Utility.Show();
                
                bool hasConnection = DependencyService.Get<Intefaces.IConnectivityChecker>().DeviceHasInternet();

                if (hasConnection)
                {
                    btnAutoComplete.Image = "autocomplete.png";
                }
                else
                {
                    Utility.Hide();
                    await DisplayAlert("No Conexion", "Debe estar conectado a internet para usar esta opcion.", "Ok");                    
                    return;
                }



                var app = Application.Current as App;
                bool _onlyCompleteMissingInfo = app.OnlyCompleteMissingInfo;
                SongInfoAudioDB.SongObject result = null;

                if (_onlyCompleteMissingInfo)
                {
                    //Solo actualizo los campos vacios
                    //TENGO QUE HACER LA LOGICA PARA REEMPLAZAR LOS CARACTERES QUE PUEDA TENER EL NOMBRE DE
                    //LA CANCION Y EL ARTISTA
                    if (!string.IsNullOrEmpty(txtSongTitle.Text.Replace(" ", "_")) && !string.IsNullOrEmpty(txtArtist.Text))
                    {
                        result = await Utility.getSongDataInfo_AudioDB(txtSongTitle.Text.Replace(" ", "_"), txtArtist.Text);
                    }
                    else if (!string.IsNullOrEmpty(txtSongTitle.Text.Replace(" ", "_")))
                    {
                        result = await Utility.getSongDataInfo_AudioDB(txtSongTitle.Text.Replace(" ", "_"));
                    }
                    else if (!string.IsNullOrEmpty(txtArtist.Text))
                    {
                        result = await Utility.getSongDataInfo_AudioDB(txtArtist.Text);
                    }

                }
                else
                {
                    //Actualizo toda la informaciones
                }

                if (result != null && result.track != null)
                {
                    txtArtist.Text = result.track.FirstOrDefault().strArtist;
                    txtAlbum.Text = result.track.FirstOrDefault().strAlbum;
                    txtGenre.Text = result.track.FirstOrDefault().strGenre;


                    Utility.Hide();
                }
                else
                {
                    Utility.Hide();
                    await DisplayAlert("No Encontrado", "Informacion no encontrada.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Utility.Hide();

                throw;
            }
        }



        /*
         
                //open root folder
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                //open folder if exists

                IFolder folder = await rootFolder.CreateFolderAsync("Download", CreationCollisionOption.OpenIfExists);
                

                var path = DependencyService.Get<Intefaces.IPathService>().InternalFolder;
                var pat2h = DependencyService.Get<Intefaces.IPathService>().PrivateExternalFolder;
                var pat23 = DependencyService.Get<Intefaces.IPathService>().PublicExternalFolder;

                //open file if exists
                IFile file = await folder.GetFileAsync("Atreyu_-_The_Theft.mp3");
                //using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                //{
                //    long length = stream.Length;
                //    BackgroundModel.Image = new byte[length];
                //    stream.Read(BackgroundModel.Image, 0, (int)length);
                //}
             
             
             
         */
        public static string Directorypath
        {
            get
            {
                return "";//Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "FolderPath");
            }
        }
    }
}