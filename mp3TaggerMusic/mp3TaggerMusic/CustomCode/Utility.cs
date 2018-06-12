using mp3TaggerMusic.Intefaces;
using mp3TaggerMusic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace mp3TaggerMusic.CustomCode
{
    public static class Utility
    {
        public static string lastfmApiKey = GetLastFmApiKey(); //"9b1e45575aa97afc4f34665f46148cc5";
        //public static string lastfmUrl = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key={0}&artist={1}&track={2}&format=json";
        public static string lastfmUrl = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key={0}&{1}{2}&format=json";

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
                    realSongTitle = string.Format("&track ={0}", System.Net.WebUtility.UrlEncode(songTitle));

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

        
        public static void Show()
        {
            DependencyService.Get<IProgressInterface>().Show();
        }

        public static void Hide()
        {
            DependencyService.Get<IProgressInterface>().Hide();
        }

        public static string GetLastFmApiKey()
        {
            return GetBaseKey("lastfm-api-key");
        }

        static string GetBaseKey(string configKey)
        {
            XDocument doc = XDocument.Load("appSettings.config.xml");
            var configKeyValue = doc.Element("config").Element(configKey).Value;
                                      //select new
                                      //{
                                      //    Title = s.Attribute("Title").Value,
                                      //    Artist = s.Attribute("Artist").Value,
                                      //    TrackId = s.Attribute("TrackId").Value,
                                      //    Timestamp = s.Attribute("Timestamp").Value
                                      //};


            return configKeyValue;

            //var assembly = mp3TaggerMusic.GetTypeInfo(typeof(LoadResourceText)).Assembly;
            //Stream stream = assembly.GetManifestResourceStream("WorkingWithFiles.PCLTextResource.txt");

            //var type = typeof(Setup);
            //var resource = string.Format("{0}.config.xml", type.Namespace);

            //using (var stream = type.Assembly.GetManifestResourceStream(resource))
            //using (var reader = new StreamReader(stream))
            //{
            //    var doc = XDocument.Parse(reader.ReadToEnd());

            //    return doc.Element("config").Element(configKey).Value;
            //}
        }
    }

}
