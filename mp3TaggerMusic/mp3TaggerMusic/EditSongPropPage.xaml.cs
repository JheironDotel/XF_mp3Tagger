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
        private Stream streamW;

        public EditSongPropPage(FileData pickedFile = null)
        {
            InitializeComponent();

            _pickedFile = pickedFile;
            FillSongInfo(pickedFile);
        }

        public async void FillSongInfo(FileData pickedFile)
        {
            try
            {
                var name = pickedFile.FileName;
                var path = pickedFile.FilePath;

                var fExtension = Path.GetExtension(name);
                if (!Utility.AudioExtensionList.Contains(fExtension))
                {
                    string msj = string.Format("Los tipos de archivos de audio aceptados son: {0}", string.Join(", ", Utility.AudioExtensionList));
                    await DisplayAlert("Archivo Audio", msj, "Ok");
                    return;
                }

                //var path =pickedFile.FilePath;
                byte[] fileData = pickedFile.DataArray;
                //Stream stream = new MemoryStream(fileData);

                streamR = new MemoryStream();
                //streamR.Write(fileData, 0, fileData.Length);

                //streamW = new MemoryStream();
                //streamW.Write(fileData, 0, fileData.Length);


                IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(path);//

                var file = await folder.GetFileAsync(name);

                //var task = file.OpenAsync(FileAccess.ReadAndWrite);
                //streamR = task.Result;

                streamR = await file.OpenAsync(FileAccess.ReadAndWrite);

                TagLib.Id3v2.Tag.DefaultVersion = 3; TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                var tagFile = //TagLib.File.Create(new TagLib.StreamFileAbstraction(name, streamR, streamW));
                    TagLib.File.Create(new FileAbstraction(name, streamR));
                var tags = tagFile.GetTag(TagLib.TagTypes.Id3v2);

                if (tags != null)
                {
                    var realArtist = tags.AlbumArtists.Count() > 0 ? tags.AlbumArtists : tags.Performers;

                    txtSongTitle.Text = tags.Title;
                    txtArtist.Text = string.Join(", ", realArtist);
                    txtAlbum.Text = tags.Album;
                    txtTrackNo.Text = tags.Track.ToString();
                    txtYear.Text = tags.Year.ToString();
                    txtGenre.Text = string.Join(", ", tags.Genres);

                    if (tags.Pictures.Count() > 0)
                    {
                        var cover = tags.Pictures.FirstOrDefault().Data.Data;
                        Stream coverStream = new MemoryStream(cover);

                        imgCoverArt.Source = ImageSource.FromStream(() => coverStream);
                    }
                    else
                    {
                        imgCoverArt.Source = ImageSource.FromResource("coverart.png");
                    }
                }
            }
            catch (Exception ex)
            {
                string msj = string.Format("Ha ocurrido un error: {0}", ex);
                await DisplayAlert("Error", msj, "Ok");
            }
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                var name = _pickedFile.FileName;
                //var path = _pickedFile.FilePath;
                byte[] fileData = _pickedFile.DataArray;
                //Stream streamR = new MemoryStream(fileData);
                //Stream streamW = new MemoryStream(fileData);

                //MemoryStream streamR = new MemoryStream();
                //streamR.Write(fileData, 0, fileData.Length);

                //MemoryStream streamW = new MemoryStream();
                //streamW.Write(fileData, 0, fileData.Length);

                TagLib.Id3v2.Tag.DefaultVersion = 3; TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                using (var tagFile = TagLib.File.Create(new FileAbstraction(name, streamR)/*TagLib.File.Create(new TagLib.StreamFileAbstraction(name, streamR, streamW)*/))
                {
                    //tagFile.RemoveTags(TagLib.TagTypes.Id3v1 | TagLib.TagTypes.Id3v2);

                    var tags = tagFile.Tag;//tagFile.GetTag(TagLib.TagTypes.Id3v2, true);

                    string[] realArtist = new string[] { };
                    string[] realGenres = new string[] { };
                    if (!string.IsNullOrEmpty(txtArtist.Text))
                    {
                        var sp = txtArtist.Text.Split(',');

                        if (sp.Count() > 0)
                        {
                            realArtist = sp;
                        }
                    }

                    if (!string.IsNullOrEmpty(txtGenre.Text))
                    {
                        var sp = txtGenre.Text.Split(',');

                        if (sp.Count() > 0)
                        {
                            realGenres = sp;
                        }
                    }

                    //tagFile.Tag.Title = txtSongTitle.Text;
                    tags.Title = txtSongTitle.Text;
                    //tagFile.Tag.Performers = realArtist;
                    //tagFile.Tag.AlbumArtists = realArtist;
                    //tagFile.Tag.Album = txtAlbum.Text;
                    //tagFile.Tag.Track = txtTrackNo.Text.ToUInt();
                    //tagFile.Tag.Year = txtYear.Text.ToUInt();
                    //tagFile.Tag.Genres = realGenres;

                    if (changedCover)
                    {
                        Stream coverStream = new MemoryStream(changedCoverImg);
                        var tagCover = new TagLib.StreamFileAbstraction("hola", coverStream, coverStream);
                        tagFile.Tag.Pictures = new[] { new TagLib.Picture(tagCover) };
                    }
                    tagFile.Save();
                    tagFile.Dispose();
                    streamW.Dispose();
                }
                //stm.Position = 0;
                //using (var reader = new BinaryReader(stm))
                //{
                //    reader.ReadBytes((int)stm.Length);
                //}
            }
            catch (Exception ex)
            {
                string msj = string.Format("Ha ocurrido un error: {0}", ex);
                DisplayAlert("Error", msj, "Ok");
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