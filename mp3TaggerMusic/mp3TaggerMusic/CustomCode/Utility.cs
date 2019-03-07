using mp3TaggerMusic.Intefaces;
using mp3TaggerMusic.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace mp3TaggerMusic.CustomCode
{
    public static class Utility
    {
        public static string lastfmApiKey = "9b1e45575aa97afc4f34665f46148cc5";
        public static string audioDbApiKey = "195068";
        //public static string lastfmUrl = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key={0}&artist={1}&track={2}&format=json";
        public static string lastfmUrl = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key={0}&{1}{2}&format=json";
        public static string audioDbUrlTrackInfo = "http://www.theaudiodb.com/api/v1/json/{0}/searchtrack.php?{1}{2}";
        public static string audioDbUrlAlbumInfo = "http://www.theaudiodb.com/api/v1/json/{0}/album.php?m={1}";


        public static string[] imgExtensionList = new[]
        {
            ".jpg", ".png", ".bmp", ".jpeg"
        };

        public static string[] AudioExtensionList = new[]
        {
            ".mp3", ".flac", ".aac", ".wma", ".m4a"
        };


        public static async Task<SongInfoClass.SongObject> getSongDataInfo(string songTitle = "", string songArtist = "")
        {
            using (System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient())
            {
                string realUrl = "";
                string realSongArtist = "";
                string realSongTitle = "";


                if (!string.IsNullOrEmpty(songTitle) && !string.IsNullOrEmpty(songArtist))
                {
                    realSongArtist = string.Format("artist={0}", System.Net.WebUtility.UrlEncode(songArtist));
                    realSongTitle = string.Format("&track={0}", System.Net.WebUtility.UrlEncode(songTitle));

                    realUrl = string.Format(lastfmUrl, lastfmApiKey, realSongArtist, realSongTitle);
                }
                else if (!string.IsNullOrEmpty(songTitle))
                {
                    realSongTitle = string.Format("track={0}", System.Net.WebUtility.UrlEncode(songTitle));

                    realUrl = string.Format(lastfmUrl, lastfmApiKey, realSongTitle);
                }
                else if (!string.IsNullOrEmpty(songArtist))
                {
                    realSongArtist = string.Format("artist={0}", System.Net.WebUtility.UrlEncode(songArtist));
                    realUrl = string.Format(lastfmUrl, lastfmApiKey, songArtist);
                }

                var content = await _client.GetStringAsync(realUrl);

                var songData = Newtonsoft.Json.JsonConvert.DeserializeObject<SongInfoClass.SongObject>(content);


                return songData;

                //var posts = JsonConvert.DeserializeObject<List<Post>>(content);
                //_posts = new ObservableCollection<Post>(posts);
                //postsListView.ItemsSource = _posts;
            }
        }

        public static async Task<List<SongInfoAudioDB.RealSongInfo>> getSongDataInfo_AudioDB(string songTitle = "", string songArtist = "")
        {
            var realSongData = new List<SongInfoAudioDB.RealSongInfo>();
            try
            {
                using (System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient())
                {
                    string realUrl = "";
                    string realSongArtist = "";
                    string realSongTitle = "";

                    //http://www.theaudiodb.com/api/v1/json/195068/searchtrack.php?s=atreyu&t=the_theft
                    //http://www.theaudiodb.com/api/v1/json/195068/album.php?m=2122786                

                    if (!string.IsNullOrEmpty(songTitle) && !string.IsNullOrEmpty(songArtist))
                    {
                        realSongArtist = string.Format("s={0}", System.Net.WebUtility.UrlEncode(songArtist));
                        realSongTitle = string.Format("&t={0}", System.Net.WebUtility.UrlEncode(songTitle));

                        realUrl = string.Format(audioDbUrlTrackInfo, audioDbApiKey, realSongArtist, realSongTitle);
                    }
                    else if (!string.IsNullOrEmpty(songTitle))
                    {
                        realSongTitle = string.Format("t={0}", System.Net.WebUtility.UrlEncode(songTitle));

                        realUrl = string.Format(audioDbUrlTrackInfo, audioDbApiKey, realSongTitle, "");
                    }
                    else if (!string.IsNullOrEmpty(songArtist))
                    {
                        realSongArtist = string.Format("s={0}", System.Net.WebUtility.UrlEncode(songArtist));
                        realUrl = string.Format(audioDbUrlTrackInfo, audioDbApiKey, songArtist);
                    }

                    var content = await _client.GetStringAsync(realUrl);

                    var songData = Newtonsoft.Json.JsonConvert.DeserializeObject<SongInfoAudioDB.SongObject>(content);


                    string albumCover = "";
                    string albumYear = "";
                    if (songData.track.FirstOrDefault() != null && !songData.track.FirstOrDefault().idAlbum.MyStringIsNullOrEmpty())
                    {
                        foreach (var sinfo in songData.track)
                        {
                            var albumid = sinfo.idAlbum;

                            realUrl = string.Format(audioDbUrlAlbumInfo, audioDbApiKey, albumid);
                            content = await _client.GetStringAsync(realUrl);

                            var albumData = Newtonsoft.Json.JsonConvert.DeserializeObject<SongInfoAudioDB.SongAlbumObject>(content);

                            foreach (var ainfo in albumData.album)
                            {
                                albumCover = ainfo.strAlbumThumb;
                                albumYear = ainfo.intYearReleased;
                            }

                            realSongData.Add(new SongInfoAudioDB.RealSongInfo()
                            {
                                songName = sinfo.strTrack,
                                songArtist = sinfo.strArtist,
                                albumName = sinfo.strAlbum,
                                songGenre = sinfo.strGenre,
                                songTrackNumber = sinfo.intTrackNumber,

                                albumYearReleased = albumYear,
                                AlbumCoverThumb = albumCover
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return realSongData;
        }

        public static void Show()
        {
            DependencyService.Get<IProgressInterface>().Show();
        }

        public static void Hide()
        {
            DependencyService.Get<IProgressInterface>().Hide();
        }

        public static async Task<SongFilesData> BasicInfoSong(string filePath)
        {
            SongFilesData sfd = new SongFilesData();
            Stream streamR = new MemoryStream();

            var folder = await PCLStorage.FileSystem.Current.GetFileFromPathAsync(filePath);

            var filename = Path.GetFileName(filePath);

            //using (
            streamR = await folder.OpenAsync(PCLStorage.FileAccess.Read);//)
                                                                         //{
            TagLib.Id3v2.Tag.DefaultVersion = 3; TagLib.Id3v2.Tag.ForceDefaultVersion = true;

            var tagFile = TagLib.File.Create(new FileAbstraction(filePath, streamR));
            var tags = tagFile.GetTag(TagLib.TagTypes.Id3v2);

            if (tags != null)
            {
                var realArtist = tags.AlbumArtists.Count() > 0 ? tags.AlbumArtists : tags.Performers;

                sfd.AlbumName = tags.Album;
                sfd.ArtistName = string.Join(", ", realArtist);
                sfd.SongFilePath = filePath;

                if (!string.IsNullOrEmpty(tags.Title))
                {
                    sfd.SongName = tags.Title;
                }
                else
                {
                    sfd.SongName = filename;
                }

                if (tags.Pictures.Count() > 0)
                {
                    var cover = tags.Pictures.FirstOrDefault().Data.Data;
                    Stream coverStream = new MemoryStream(cover);
                    sfd.AlbumCover = ImageSource.FromStream(() => coverStream);                    
                }
                else
                {
                    var imgSource = ImageSource.FromFile("coverart.png");
                    sfd.AlbumCover = imgSource;
                }
            }
            //}

            //streamR.Dispose();
            return sfd;
        }

        public static bool DeviceHasInternet()
        {
            return DependencyService.Get<Intefaces.IConnectivityChecker>().DeviceHasInternet();
        }
    }

}
