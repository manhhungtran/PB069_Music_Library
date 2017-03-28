using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base;

namespace ApiLogic
{
    public class LibraryManager : ILibraryManager
    {
        internal readonly Library _library;
        internal ISongManager SongManager;
        internal IPlaylistManager PlaylistManager;

        public LibraryManager()
        {
            _library = new Library();
            SongManager = new SongManager();
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

            _library.RootPath = new HashSet<string>(paths.ToList());
        }

        public void InitializeLibrary()
        {
            foreach (string s in _library.RootPath)
            {
                _library.Songs.UnionWith(Directory.EnumerateFiles(s, "*.mp3", SearchOption.AllDirectories));
            }

            foreach (string potentialSong in _library.Songs)
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

            var parent = _library.RootPath.Where(directoryPath.IsSubPathOf);
            var children = _library.RootPath.Where(song => song.IsSubPathOf(directoryPath)).ToList();

            if (parent.Any())
            {
                throw new DirectoryIsSubPathException();
            }

            if (children.Any())
            {
                foreach (string child in children)
                {
                    _library.RootPath.Remove(child);
                }
            }

            _library.RootPath.Add(directoryPath);
            InitializeLibrary();
        }

        public void RemoveRootPath(string path)
        {
            _library.RootPath.Remove(path);
        }
    }
}
