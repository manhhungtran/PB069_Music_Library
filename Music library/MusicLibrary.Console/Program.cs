using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Base;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace MusicLibrary.Console
{

    public class Program
    {
        static void Main(string[] args)
        {

            //var FileSystem = new PhysicalFileSystem(AppDomain.CurrentDomain.BaseDirectory);
            //var worker = new XMLImportExport<Song>(FileSystem);

            //string asf = String.Empty;
            //var files = Directory.EnumerateFiles(".\\zTest", "*", SearchOption.AllDirectories).Select(Path.GetFullPath);

            //var neco = files.Select(x => Song.New(x));
            ////worker.Export(new List<Song> {new Song {Path = "asfkasbg"}});
            ////var neco =
            ////    FileSystem.OpenFile(FileSystemPath.Root.AppendFile($"{nameof(List<Song>)}.xml"), FileAccess.Read)
            ////        .ReadAllBytes();

            ////FileSystem.Dispose();

            ////FileStream file = File.Create(@"C:\neco.txt");
            ////foreach (byte[] bytes in neco.Select(x => Encoding.UTF8.GetBytes(x)))
            ////{
            ////    file.Write(bytes);
            ////}
            ////file.Close();

            //foreach (Song asfs in neco)
            //{
            //    System.Console.WriteLine(asfs);
            //}

        }

    }
}