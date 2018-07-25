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
using System.IO;
using Android.Provider;

namespace mp3TaggerMusic.Droid
{
    public class ArrayFiles
    {
        public ArrayFiles() { }

        public IEnumerable<string> getfile(string path)
        {
            //AllAudioFiles();

            List<string> faaa = new List<string>();
            var file = System.IO.Directory.GetFiles(path);
            if (file.Any())
            {
                foreach (var item in file)
                {
                    faaa.Add(item);
                }
            }
            /*
            IEnumerable<File> listFile = dir.GetDirectories();
            if (listFile != null && listFile.length > 0)
            {
                for (int i = 0; i < listFile.length; i++)
                {

                    if (listFile[i].isDirectory())
                    {
                        fileList.add(listFile[i]);
                        getfile(listFile[i]);

                    }
                    else
                    {
                        if (listFile[i].getName().endsWith(".png")
                                || listFile[i].getName().endsWith(".jpg")
                                || listFile[i].getName().endsWith(".jpeg")
                                || listFile[i].getName().endsWith(".gif"))

                        {
                            fileList.add(listFile[i]);
                        }
                    }

                }
            }*/
            return faaa;
        }


        public IEnumerable<string> AllAudioFiles(string[] AudioExtensionList)
        {
            var projection = new[] { MediaStore.Audio.Media.InterfaceConsts.Id, MediaStore.Audio.Media.InterfaceConsts.Data };
            var cursor = Application.Context.ContentResolver.Query(MediaStore.Audio.Media.ExternalContentUri, projection,
                null, null, MediaStore.Audio.Media.InterfaceConsts.Id);

            var pathIndex = cursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Data);

            //System.Diagnostics.Debug.WriteLine($"Got {cursor.Count} Audio");

            var AudioPaths = new List<string>();

            while (cursor.MoveToNext())
            {
                var path = cursor.GetString(pathIndex);            

                var fExtension = Path.GetExtension(path);
                if (AudioExtensionList.Contains(fExtension))
                {
                    AudioPaths.Add(path);
                }
            }

            return AudioPaths;            
        }
    }
}