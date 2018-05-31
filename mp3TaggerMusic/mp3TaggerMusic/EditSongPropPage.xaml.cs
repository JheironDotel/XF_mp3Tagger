using PCLStorage;

using Plugin.FilePicker.Abstractions;
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
    public partial class EditSongPropPage : ContentPage
    {
        private FileData _pickedFile;

        public EditSongPropPage(FileData pickedFile)
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
                //var path =pickedFile.FilePath;
                byte[] fileData = pickedFile.DataArray;
                System.IO.Stream stream = new System.IO.MemoryStream(fileData);

                var tagFile = TagLib.File.Create(new TagLib.StreamFileAbstraction(name, stream, stream));
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
                        System.IO.Stream coverStream = new System.IO.MemoryStream(cover);

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

            }
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                var name = _pickedFile.FileName;
                //var path =pickedFile.FilePath;
                byte[] fileData = _pickedFile.DataArray;
                System.IO.Stream stream = new System.IO.MemoryStream(fileData);

                var tagFile = TagLib.File.Create(new TagLib.StreamFileAbstraction(name, stream, stream));

                if (tagFile != null)
                {
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

                    tagFile.Tag.Title = txtSongTitle.Text;
                    tagFile.Tag.Performers = realArtist;
                    tagFile.Tag.AlbumArtists = realArtist;
                    tagFile.Tag.Album = txtAlbum.Text;
                    tagFile.Tag.Track = txtTrackNo.Text.ToUInt();
                    tagFile.Tag.Year = txtYear.Text.ToUInt();
                    tagFile.Tag.Genres = realGenres;


                    tagFile.Save();

                    if (tags.Pictures.Count() > 0)
                    {
                        var cover = tags.Pictures.FirstOrDefault().Data.Data;
                        System.IO.Stream coverStream = new System.IO.MemoryStream(cover);

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
                //using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
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
                return "";//System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "FolderPath");
            }
        }
    }
}