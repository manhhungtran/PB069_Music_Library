using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base;

namespace ApiLogic
{
    public class LibraryManager : ILibraryManager
    {
        private static string LIBRARY_INFO_ROOT_PATH = "Library.xml";

        internal Library Library;
        internal ISongManager SongManager;
        internal IPlaylistManager PlaylistManager;
        internal IImportExport<Library> ImportExport;

        public LibraryManager()
        {
            SongManager = new SongManager();
            ImportExport = new XMLImportExport<Library>();

            if (File.Exists(LIBRARY_INFO_ROOT_PATH))
            {
                try
                {
                    Library = ImportExport.Import(LIBRARY_INFO_ROOT_PATH).First();
                }
                catch
                {
                    Library = new Library();
                }
            }
            else
            {
                Library = new Library();
            }
        }

        public LibraryManager(IEnumerable<string> paths) : this()
        {
            if (paths == null)
            {
                throw new NullReferenceException(nameof(paths));
            }

            foreach (string path in paths)
            {
                if (!Directory.Exists(path))
                {
                    throw new DirectoryNotFoundException(nameof(path));
                }
            }

            Library.RootPath = new HashSet<string>(paths.ToList());
        }

        public void InitializeLibrary()
        {
            SongManager.ClearSongs();
            Library.Songs.Clear();

            foreach (string s in Library.RootPath)
            {
                Library.Songs.UnionWith(Directory.EnumerateFiles(s, "*.mp3", SearchOption.AllDirectories));
            }

            foreach (string potentialSong in Library.Songs)
            {
                SongManager.CreateSong(Song.New(potentialSong));
            }
        }

        public void AddRootPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }

            var parent = Library.RootPath.Where(directoryPath.IsSubPathOf);
            var children = Library.RootPath.Where(song => song.IsSubPathOf(directoryPath)).ToList();

            if (parent.Any())
            {
                throw new DirectoryIsSubPathException();
            }

            if (children.Any())
            {
                foreach (string child in children)
                {
                    Library.RootPath.Remove(child);
                }
            }

            Library.RootPath.Add(directoryPath);
            InitializeLibrary();
        }

        public void RemoveRootPath(string path)
        {
            Library.RootPath.Remove(path);
            InitializeLibrary();
        }

        public ISongManager GetSongManager()
        {
            return SongManager;
        }

        public void Save()
        {
            ImportExport.Export(new HashSet<Library> { Library }, LIBRARY_INFO_ROOT_PATH);
        }

        public void Load(string filePath)
        {
            Library = ImportExport.Import(filePath).First();
        }

        public List<string> GetAllRoots()
        {
            return new List<string>(Library.RootPath);
        }
    }
}
