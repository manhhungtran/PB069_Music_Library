using System;
using System.IO;
using System.Text;

using TagLib;

namespace Base
{
    public class Song
    {
        public string Name { get; internal set; }
        public string Path { get; internal set; }
        public string Title { get; internal set; }
        public long Lenght { get; internal set; }
        public string Album { get; internal set; }
        public uint Year { get; internal set; }
        public string Artist { get; internal set; }


        /// <summary>
        /// Creates new instance of Song and maps all IDv info about it to the class.
        /// </summary>
        public static Song New(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            var reader = System.IO.File.Open(filePath, FileMode.Open);

            var sfa = new StreamFileAbstraction(reader.Name, reader, reader);
            var song = new Song();

            TagLib.File tagFile = TagLib.File.Create(sfa);

            song.Name = System.IO.Path.GetFileNameWithoutExtension(filePath);
            song.Path = filePath;
            song.Artist = tagFile.Tag.FirstAlbumArtist;
            song.Album = tagFile.Tag.Album;
            song.Title = tagFile.Tag.Title + " " ;
            song.Year =  tagFile.Tag.Year;
            song.Lenght = tagFile.Length;

            return song;
        }

        protected bool Equals(Song other)
        {
            return string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Song) obj);
        }

        public override int GetHashCode()
        {
            return (Path != null ? Path.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"\n{nameof(Path)}: {Path}, " +
                   $"\n{nameof(Lenght)}: {Lenght}, " +
                   $"\n{nameof(Title)}: {Title}, " +
                   $"\n{nameof(Album)}: {Album}, " +
                   $"\n{nameof(Year)}: {Year}, " +
                   $"\n{nameof(Artist)}: {Artist}," +
                   $"\n{nameof(Name)}: {Name}";
        }
    }
}