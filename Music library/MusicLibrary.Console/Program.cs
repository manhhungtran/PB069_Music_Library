using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Base;
using SharpFileSystem;
using SharpFileSystem.FileSystems;
using SharpFileSystem.IO;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace MusicLibrary.Console
{

    public class Program
    {
        static void Main(string[] args)
        {

            var FileSystem = new PhysicalFileSystem("C:\\");
            var worker = new XMLImportExport<Song>(FileSystem);

            worker.Export(new List<Song> {new Song {Path = "asfkasbg"}});
            var neco =
                FileSystem.OpenFile(FileSystemPath.Root.AppendFile($"{nameof(List<Song>)}.xml"), FileAccess.Read)
                    .ReadAllBytes();

            FileSystem.Dispose();

            FileStream file = File.Create(@"C:\neco.txt");
            file.Write(neco);

        }

    }
}