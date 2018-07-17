using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3TaggerMusic.Models
{
    public class SongInfoAudioDB
    {
        public class Track
        {
            public string idTrack { get; set; }
            public string idAlbum { get; set; }
            public string idArtist { get; set; }
            public string idLyric { get; set; }
            public object idIMVDB { get; set; }
            public string strTrack { get; set; }
            public string strAlbum { get; set; }
            public string strArtist { get; set; }
            public object strArtistAlternate { get; set; }
            public object intCD { get; set; }
            public string intDuration { get; set; }
            public string strGenre { get; set; }
            public object strMood { get; set; }
            public object strStyle { get; set; }
            public object strTheme { get; set; }
            public object strDescriptionEN { get; set; }
            public object strTrackThumb { get; set; }
            public object strTrackLyrics { get; set; }
            public object strMusicVid { get; set; }
            public object strMusicVidDirector { get; set; }
            public object strMusicVidCompany { get; set; }
            public object strMusicVidScreen1 { get; set; }
            public object strMusicVidScreen2 { get; set; }
            public object strMusicVidScreen3 { get; set; }
            public object intMusicVidViews { get; set; }
            public object intMusicVidLikes { get; set; }
            public object intMusicVidDislikes { get; set; }
            public object intMusicVidFavorites { get; set; }
            public object intMusicVidComments { get; set; }
            public string intTrackNumber { get; set; }
            public string intLoved { get; set; }
            public object intScore { get; set; }
            public object intScoreVotes { get; set; }
            public string strMusicBrainzID { get; set; }
            public string strMusicBrainzAlbumID { get; set; }
            public string strMusicBrainzArtistID { get; set; }
            public string strLocked { get; set; }
        }

        public class SongObject
        {            public List<Track> track { get; set; }
        }
    }
}
