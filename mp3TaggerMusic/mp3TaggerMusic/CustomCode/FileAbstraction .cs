﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3TaggerMusic.CustomCode
{
   public class FileAbstraction : TagLib.File.IFileAbstraction
    {
        public FileAbstraction(string name, Stream stream)
        {
            this.Name = name;
            this.ReadStream = stream;
            this.WriteStream = stream;
        }

        public void CloseStream(Stream stream)
        {
            stream.Flush();
        }

        public string Name
        {
            get;
            private set;
        }

        public Stream ReadStream
        {
            get;
            private set;
        }

        public Stream WriteStream
        {
            get;
            private set;
        }
    }
}
