using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using mp3TaggerMusic.Intefaces;
using mp3TaggerMusic.Droid;
using System.IO;

[assembly: Dependency(typeof(FileList))]
namespace mp3TaggerMusic.Droid
{
    public class FileList : IFileList
    {
        public FileList()
        {

        }

        public IEnumerable<string> GetAllSongFiles(string[] AudioExtensionList)
        {   
            ArrayFiles arrayFiles = new ArrayFiles();
            var songsFiles = arrayFiles.AllAudioFiles(AudioExtensionList);
    
            return songsFiles;
        }


        public string GetFileFromPath()
        {
            string path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            ArrayFiles arrayFiles = new ArrayFiles();
            var a = arrayFiles.getfile(path);

            /*Path.Combine(
        System.Environment.GetFolderPath(System.Environment..MyDocuments),
        fileName);*/

            return path;
        }
    }
}