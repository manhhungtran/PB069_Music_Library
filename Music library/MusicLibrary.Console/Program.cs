using System;
using System.IO;
using System.Text;

using Base;

namespace MusicLibrary.Console
{

    public class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            var ong = Song.New("C:\\Users\\Hung\\Music\\Chinaski - Každý ráno (lyrics D ).mp3");

            System.Console.WriteLine(ong);
        }

    }
}