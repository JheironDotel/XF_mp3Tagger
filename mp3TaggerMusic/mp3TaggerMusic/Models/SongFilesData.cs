using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3TaggerMusic.Models
{
    public class SongFilesData
    {
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public Xamarin.Forms.ImageSource AlbumCover { get; set; }
        public string SongFilePath { get; set; }
        public bool Selected { get; set; }
    }
}
