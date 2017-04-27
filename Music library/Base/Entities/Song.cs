using System;
using System.IO;
using System.Xml.Serialization;
using TagLib;
using File = System.IO.File;

namespace Base
{
    [Serializable]
    public class Song
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Path))]
        public string Path { get; set; }

        [XmlElement(nameof(Title))]
        public string Title { get; set; }

        [XmlElement(nameof(Lenght))]
        public long Lenght { get; set; }

        [XmlElement(nameof(Album))]
        public string Album { get; set; }

        [XmlElement(nameof(Year))]
        public uint Year { get; set; }

        [XmlElement(nameof(Artist))]
        public string Artist { get; set; }

        /// <summary>
        /// Creates new instance of Song and maps all IDv info about it to the class.
        /// </summary>
        public static Song New(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(filePath));
            }

            var reader = File.Open(filePath, FileMode.Open);

            var streamFileAbstraction = new StreamFileAbstraction(filePath, reader, reader);

            var tagFile = TagLib.File.Create(streamFileAbstraction);

            return new Song
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(filePath),
                Path = filePath,
                Artist = tagFile.Tag.FirstAlbumArtist,
                Album = tagFile.Tag.Album,
                Title = tagFile.Tag.Title,
                Year = tagFile.Tag.Year,
                Lenght = tagFile.Length
            };
        }

        protected bool Equals(Song other)
        {
            return string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Song) obj);
        }

        public override int GetHashCode()
        {
            return Path?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public string ToStringAll()
        {
            return $"{nameof(Name)}: {Name}, " +
                   $"\n{nameof(Path)}: {Path}, " +
                   $"\n{nameof(Title)}: {Title}, " +
                   $"\n{nameof(Lenght)}: {Lenght}, " +
                   $"\n{nameof(Album)}: {Album}, " +
                   $"\n{nameof(Year)}: {Year}, " +
                   $"\n{nameof(Artist)}: {Artist}";
        }
    }
}