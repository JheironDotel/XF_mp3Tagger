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

        public class Album
        {
            public string idAlbum { get; set; }
            public string idArtist { get; set; }
            public object idLabel { get; set; }
            public string strAlbum { get; set; }
            public string strAlbumStripped { get; set; }
            public string strArtist { get; set; }
            public string strArtistStripped { get; set; }
            public string intYearReleased { get; set; }
            public string strStyle { get; set; }
            public string strGenre { get; set; }
            public object strLabel { get; set; }
            public string strReleaseFormat { get; set; }
            public string intSales { get; set; }
            public string strAlbumThumb { get; set; }
            public object strAlbumThumbHQ { get; set; }
            public object strAlbumThumbBack { get; set; }
            public string strAlbumCDart { get; set; }
            public object strAlbumSpine { get; set; }
            public object strAlbum3DCase { get; set; }
            public object strAlbum3DFlat { get; set; }
            public object strAlbum3DFace { get; set; }
            public string strDescriptionEN { get; set; }
            public object strDescriptionDE { get; set; }
            public object strDescriptionFR { get; set; }
            public object strDescriptionCN { get; set; }
            public object strDescriptionIT { get; set; }
            public object strDescriptionJP { get; set; }
            public object strDescriptionRU { get; set; }
            public object strDescriptionES { get; set; }
            public object strDescriptionPT { get; set; }
            public object strDescriptionSE { get; set; }
            public object strDescriptionNL { get; set; }
            public object strDescriptionHU { get; set; }
            public object strDescriptionNO { get; set; }
            public object strDescriptionIL { get; set; }
            public object strDescriptionPL { get; set; }
            public object intLoved { get; set; }
            public object intScore { get; set; }
            public object intScoreVotes { get; set; }
            public string strReview { get; set; }
            public string strMood { get; set; }
            public object strTheme { get; set; }
            public object strSpeed { get; set; }
            public object strLocation { get; set; }
            public string strMusicBrainzID { get; set; }
            public string strMusicBrainzArtistID { get; set; }
            public object strAllMusicID { get; set; }
            public object strBBCReviewID { get; set; }
            public object strRateYourMusicID { get; set; }
            public string strDiscogsID { get; set; }
            public string strWikidataID { get; set; }
            public string strWikipediaID { get; set; }
            public object strGeniusID { get; set; }
            public object strLyricWikiID { get; set; }
            public object strMusicMozID { get; set; }
            public object strItunesID { get; set; }
            public object strAmazonID { get; set; }
            public string strLocked { get; set; }
        }

        public class SongObject
        {
            public List<Track> track { get; set; }
        }

        public class SongAlbumObject
        {
            public List<Album> album { get; set; }
        }

        public class RealSongInfo
        {
            public string songName { get; set; }
            public string songArtist { get; set; }
            public string albumName { get; set; }
            public string songTrackNumber { get; set; }
            public string albumYearReleased { get; set; }
            public string songGenre { get; set; }
            public string AlbumCoverThumb { get; set; }


        }


    }
}
