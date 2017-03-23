using System;
using System.IO;
using System.Text;


using SharpFileSystem;
using SharpFileSystem.FileSystems;
using TagLib;

namespace Base
{
    public class Song
    {
        private static IFileSystem _fileSystem;

        public static IFileSystem FileSystem
        {
            get { return _fileSystem ?? new PhysicalFileSystem("C:/"); }
            set { _fileSystem = value; }
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Title { get; set; }

        public long Lenght { get; set; }

        public string Album { get; set; }

        public uint Year { get; set; }

        public string Artist { get; set; }

        /// <summary>
        /// Creates new instance of Song and maps all IDv info about it to the class.
        /// </summary>
        public static Song New(string filePath, IFileSystem fileSystem = null)
        {
            if (fileSystem != null)
            {
                FileSystem = fileSystem;
            }

            if (!FileSystem.Exists(FileSystemPath.Parse(filePath)))
            {
                throw new FileNotFoundException(nameof(filePath));
            }

            var reader = FileSystem.OpenFile(FileSystemPath.Parse(filePath), FileAccess.ReadWrite);

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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Song) obj);
        }

        public override int GetHashCode()
        {
            return Path?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Mainly just for testing purpose.
        /// </summary>
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